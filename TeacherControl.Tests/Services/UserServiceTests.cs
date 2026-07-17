using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using TeacherControl.DTOs.Requests;
using TeacherControl.Enums;
using TeacherControl.Extensions;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services;

namespace TeacherControl.Tests.Services;

public class UserServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IRoleRepository> _roleRepository;
    private readonly Mock<IPasswordHasher<User>> _passwordHasher;
    private readonly UserService _service;

    // Valores válidos e conhecidos, para podermos afirmar sobre eles.
    private const string Email = "new.user@gmail.com"; // já minúsculo => Email.ToLower() == Email
    private const string PlainPassword = "plain-password";
    private const string HashedPassword = "HASHED_123";
    private const string Name = "New User";
    private readonly Guid _roleId = Guid.NewGuid();

    public UserServiceTests()
    {
        _fixture = new Fixture();
        _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        _roleRepository = new Mock<IRoleRepository>(MockBehavior.Strict);
        _passwordHasher = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);

        _service = new UserService(_userRepository.Object, _roleRepository.Object, _passwordHasher.Object);
    }

    private UserRequestDto BuildValidRequest() =>
        _fixture.Build<UserRequestDto>()
            .With(x => x.Email, Email)
            .With(x => x.Password, PlainPassword)
            .With(x => x.Name, Name)
            .With(x => x.RoleId, _roleId)
            .Create();

    // User possui setters privados e validação de domínio: construímos pelo construtor público.
    private User BuildUser(string email = Email, string password = HashedPassword, string name = Name, Guid? roleId = null) => new(email, password, name, roleId ?? _roleId);

    // =========================================================
    // CREATE
    // =========================================================

    [Fact]
    public async Task DadoRequestValido_QuandoCriar_EntaoDeveHashearSenhaEPersistirUsuario()
    {
        // Arrange
        var request = BuildValidRequest();
        var role = new Role("Manager", "Manager role");

        _userRepository
            .Setup(r => r.GetUserByEmail(Email))
            .ReturnsAsync((User?)null)
            .Verifiable();

        _roleRepository
            .Setup(r => r.GetById(_roleId))
            .ReturnsAsync(role)
            .Verifiable();

        _passwordHasher
            .Setup(h => h.HashPassword(
                It.Is<User>(u => u.Email == Email && u.Name == Name && u.RoleId == _roleId),
                PlainPassword))
            .Returns(HashedPassword)
            .Verifiable();

        _userRepository
            .Setup(r => r.Add(It.Is<User>(u =>
                u.Email == Email &&
                u.Name == Name &&
                u.RoleId == _roleId &&
                u.Password == HashedPassword)))
            .ReturnsAsync((User u) => u)
            .Verifiable();

        _userRepository
            .Setup(r => r.GetById(It.Is<Guid>(id => id != Guid.Empty)))
            .ReturnsAsync(BuildUser())
            .Verifiable();

        // Act
        var result = await _service.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(Email);
        result.Name.Should().Be(Name);
        result.Status.Should().Be(UserStatus.Active.ToString());

        _userRepository.Verify(r => r.GetUserByEmail(Email), Times.Once);
        _roleRepository.Verify(r => r.GetById(_roleId), Times.Once);
        _passwordHasher.Verify(h => h.HashPassword(
            It.Is<User>(u => u.Email == Email && u.Name == Name && u.RoleId == _roleId), PlainPassword), Times.Once);
        _userRepository.Verify(r => r.Add(It.Is<User>(u => u.Password == HashedPassword)), Times.Once);
        _userRepository.Verify(r => r.GetById(It.Is<Guid>(id => id != Guid.Empty)), Times.Once);

    }

    [Fact]
    public async Task DadoEmailJaCadastrado_QuandoCriar_EntaoDeveLancarDomainException()
    {
        // Arrange
        var request = BuildValidRequest();

        _userRepository
            .Setup(r => r.GetUserByEmail(Email))
            .ReturnsAsync(BuildUser())
            .Verifiable();

        // Act
        var act = () => _service.Create(request);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .Where(e => e.Errors.Contains("Email already registered"));

        _userRepository.Verify(r => r.GetUserByEmail(Email), Times.Once);
        _roleRepository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Never);
        _passwordHasher.Verify(h => h.HashPassword(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _userRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DadoRoleInexistente_QuandoCriar_EntaoDeveLancarDomainException()
    {
        // Arrange
        var request = BuildValidRequest();

        _userRepository
            .Setup(r => r.GetUserByEmail(Email))
            .ReturnsAsync((User?)null)
            .Verifiable();

        _roleRepository
            .Setup(r => r.GetById(_roleId))
            .ReturnsAsync((Role?)null)
            .Verifiable();

        // Act
        var act = () => _service.Create(request);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .Where(e => e.Errors.Contains("Role not found"));

        _userRepository.Verify(r => r.GetUserByEmail(Email), Times.Once);
        _roleRepository.Verify(r => r.GetById(_roleId), Times.Once);
        _passwordHasher.Verify(h => h.HashPassword(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _userRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Never);

    }

    // =========================================================
    // UPDATE
    // =========================================================

    [Fact]
    public async Task DadoUsuarioValido_QuandoAtualizar_EntaoDeveAtualizarDadosEPersistir()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = BuildValidRequest();
        var existingUser = BuildUser(email: "old@gmail.com", name: "Old Name");
        var role = new Role("Manager", "Manager role");

        _userRepository.Setup(r => r.GetById(id)).ReturnsAsync(existingUser).Verifiable();
        _userRepository.Setup(r => r.GetUserByEmail(Email)).ReturnsAsync((User?)null).Verifiable();
        _roleRepository.Setup(r => r.GetById(_roleId)).ReturnsAsync(role).Verifiable();
        _passwordHasher.Setup(h => h.HashPassword(existingUser, PlainPassword)).Returns(HashedPassword).Verifiable();
        _userRepository
            .Setup(r => r.Update(It.Is<User>(u =>
                u.Email == Email &&
                u.Name == Name &&
                u.RoleId == _roleId &&
                u.Password == HashedPassword)))
            .ReturnsAsync(existingUser)
            .Verifiable();

        // Act
        var result = await _service.Update(id, request);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(Email);
        result.Name.Should().Be(Name);

        _userRepository.Verify(r => r.GetById(id), Times.Once);
        _userRepository.Verify(r => r.GetUserByEmail(Email), Times.Once);
        _roleRepository.Verify(r => r.GetById(_roleId), Times.Once);
        _passwordHasher.Verify(h => h.HashPassword(existingUser, PlainPassword), Times.Once);
        _userRepository.Verify(r => r.Update(It.Is<User>(u => u.Password == HashedPassword)), Times.Once);

    }

    [Fact]
    public async Task DadoUsuarioInexistente_QuandoAtualizar_EntaoDeveLancarDomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = BuildValidRequest();

        _userRepository.Setup(r => r.GetById(id)).ReturnsAsync((User?)null).Verifiable();

        // Act
        var act = () => _service.Update(id, request);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .Where(e => e.Errors.Contains("User not found"));

        _userRepository.Verify(r => r.GetById(id), Times.Once);
        _userRepository.Verify(r => r.GetUserByEmail(It.IsAny<string>()), Times.Never);
        _roleRepository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Never);
        _userRepository.Verify(r => r.Update(It.IsAny<User>()), Times.Never);

    }

    [Fact]
    public async Task DadoEmailDeOutroUsuario_QuandoAtualizar_EntaoDeveLancarDomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = BuildValidRequest();
        var existingUser = BuildUser();
        var otherUser = BuildUser(email: Email); // Id diferente de "id"

        _userRepository.Setup(r => r.GetById(id)).ReturnsAsync(existingUser).Verifiable();
        _userRepository.Setup(r => r.GetUserByEmail(Email)).ReturnsAsync(otherUser).Verifiable();

        // Act
        var act = () => _service.Update(id, request);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .Where(e => e.Errors.Contains("Email already registered"));

        _userRepository.Verify(r => r.GetById(id), Times.Once);
        _userRepository.Verify(r => r.GetUserByEmail(Email), Times.Once);
        _roleRepository.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Never);
        _userRepository.Verify(r => r.Update(It.IsAny<User>()), Times.Never);

    }

    [Fact]
    public async Task DadoRoleInexistente_QuandoAtualizar_EntaoDeveLancarDomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = BuildValidRequest();
        var existingUser = BuildUser();

        _userRepository.Setup(r => r.GetById(id)).ReturnsAsync(existingUser).Verifiable();
        _userRepository.Setup(r => r.GetUserByEmail(Email)).ReturnsAsync((User?)null).Verifiable();
        _roleRepository.Setup(r => r.GetById(_roleId)).ReturnsAsync((Role?)null).Verifiable();

        // Act
        var act = () => _service.Update(id, request);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .Where(e => e.Errors.Contains("Role not found"));

        _userRepository.Verify(r => r.GetById(id), Times.Once);
        _userRepository.Verify(r => r.GetUserByEmail(Email), Times.Once);
        _roleRepository.Verify(r => r.GetById(_roleId), Times.Once);
        _userRepository.Verify(r => r.Update(It.IsAny<User>()), Times.Never);

    }

    // =========================================================
    // REMOVE
    // =========================================================

    [Fact]
    public async Task DadoId_QuandoRemover_EntaoDeveDelegarParaRepositorio()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.Setup(r => r.Remove(id)).ReturnsAsync(BuildUser()).Verifiable();

        // Act
        await _service.Remove(id);

        // Assert
        _userRepository.Verify(r => r.Remove(id), Times.Once);
    }

    // =========================================================
    // GET BY ID
    // =========================================================

    [Fact]
    public async Task DadoUsuarioExistente_QuandoBuscarPorId_EntaoDeveRetornarResponse()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository
            .Setup(r => r.GetById(id))
            .ReturnsAsync(BuildUser(email: "found@gmail.com", name: "Found"))
            .Verifiable();

        // Act
        var result = await _service.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("found@gmail.com");
        result.Name.Should().Be("Found");
        result.Status.Should().Be(UserStatus.Active.ToString());

        _userRepository.Verify(r => r.GetById(id), Times.Once);
    }

    [Fact]
    public async Task DadoUsuarioInexistente_QuandoBuscarPorId_EntaoDeveLancarExcecao()
    {
        // Arrange
        var id = Guid.NewGuid();
        _userRepository.Setup(r => r.GetById(id)).ReturnsAsync((User?)null).Verifiable();

        // Act
        var act = () => _service.GetById(id);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("User not found!");
        _userRepository.Verify(r => r.GetById(id), Times.Once);
    }

    // =========================================================
    // GET ALL / GET ALL FULL
    // =========================================================

    [Fact]
    public async Task DadoUsuariosCadastrados_QuandoBuscarTodos_EntaoDeveRetornarResumo()
    {
        // Arrange
        var users = new List<User>
        {
            BuildUser(email: "a@gmail.com", name: "A"),
            BuildUser(email: "b@gmail.com", name: "B"),
        };
        _userRepository.Setup(r => r.GetAll()).ReturnsAsync(users).Verifiable();

        // Act
        var result = await _service.GetAll();

        // Assert
        result.Should().HaveCount(2);
        result.Select(u => u.Email).Should().BeEquivalentTo("a@gmail.com", "b@gmail.com");
        _userRepository.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public async Task DadoUsuariosCadastrados_QuandoBuscarTodosCompleto_EntaoDeveRetornarResponseCompleto()
    {
        // Arrange
        var users = new List<User> { BuildUser(email: "full@gmail.com", name: "Full") };
        _userRepository.Setup(r => r.GetAllFull()).ReturnsAsync(users).Verifiable();

        // Act
        var result = await _service.GetAllFull();

        // Assert
        result.Should().ContainSingle();
        result[0].Email.Should().Be("full@gmail.com");
        _userRepository.Verify(r => r.GetAllFull(), Times.Once);
    }

    // =========================================================
    // GET BY EMAIL
    // =========================================================

    [Fact]
    public async Task DadoEmailExistente_QuandoBuscarPorEmail_EntaoDeveRetornarResponse()
    {
        // Arrange
        _userRepository
            .Setup(r => r.GetUserByEmail(Email))
            .ReturnsAsync(BuildUser(email: Email))
            .Verifiable();

        // Act
        var result = await _service.GetUserByEmail(Email);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(Email);
        _userRepository.Verify(r => r.GetUserByEmail(Email), Times.Once);
    }

    [Fact]
    public async Task DadoEmailInexistente_QuandoBuscarPorEmail_EntaoDeveLancarExcecao()
    {
        // Arrange
        const string missing = "missing@gmail.com";
        _userRepository.Setup(r => r.GetUserByEmail(missing)).ReturnsAsync((User?)null).Verifiable();

        // Act
        var act = () => _service.GetUserByEmail(missing);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("User not found!");
        _userRepository.Verify(r => r.GetUserByEmail(missing), Times.Once);
    }
}