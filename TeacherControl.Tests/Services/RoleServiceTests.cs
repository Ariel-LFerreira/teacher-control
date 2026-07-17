using AutoFixture;
using FluentAssertions;
using Moq;
using TeacherControl.DTOs.Requests;
using TeacherControl.Extensions;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services;

namespace TeacherControl.Tests.Services;

public class RoleServiceTests
{
    //An automatic test object generator
    private readonly IFixture _fixture;
    
    //Mocks
    private readonly Mock<IRoleRepository> _roleRepository;
    private readonly RoleService _service;
    
    //Setei valores padrão com Const
    private const string Name = "NameRoleTests";
    private const string Description = "This is description role";
    
    
    public RoleServiceTests()
    {
        _fixture = new Fixture();
        _roleRepository = new Mock<IRoleRepository>(MockBehavior.Strict);

        _service = new RoleService(_roleRepository.Object);
    }
    
    //Método helper (criar DTO válido)
    private RoleRequestDto BuildValidRequest() =>
        _fixture.Build<RoleRequestDto>()
            .With(x => x.Name, Name)
            .With(x => x.Description, Description)
            .Create();
    
    // Role possui setters privados e validação de domínio: construímos pelo construtor público.
    private Role BuildUser(string name = Name, string description = Description) => new(name, description);
    
    [Fact]
    public async Task DadoRoleJaExistente_QuandoCriar_EntaoDeveLancarDomainException()
    {
        // Arrange
        /*var request = _fixture.Build<RoleRequestDto>()
            .With(r => r.Name, "Manager")
            .With(r => r.Description, "Manager role")
            .Create();*/
        
        // Arrange
        var request = BuildValidRequest();
        var existingRole = new Role("Manager", "Manager role");

        _roleRepository
            .Setup(r => r.GetRoleByName(request.Name))
            .ReturnsAsync(existingRole); //ROLE JÁ EXISTE

        // Act
        Func<Task> act = async () => await _service.Add(request);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .Where(e => e.Errors.Contains("Role already exists"));

        _roleRepository.Verify(r => r.GetRoleByName(request.Name), Times.Once);

        // NÃO deve continuar fluxo
        _roleRepository.Verify(r => r.Add(It.IsAny<Role>()), Times.Never);
    }
    
    [Fact]
    public async Task DadoRoleNaoExistente_QuandoCriar_EntaoDeveAdicionarComSucesso()
    {
        // Arrange
        var request = BuildValidRequest();

        _roleRepository
            .Setup(r => r.GetRoleByName(request.Name))
            .ReturnsAsync((Role?)null); 

        _roleRepository
            .Setup(r => r.Add(It.IsAny<Role>()))
            .ReturnsAsync((Role role) => role);

        // Act
        var result = await _service.Add(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);

        _roleRepository.Verify(r => r.GetRoleByName(request.Name), Times.Once);
        _roleRepository.Verify(r => r.Add(It.IsAny<Role>()), Times.Once);
    }
}