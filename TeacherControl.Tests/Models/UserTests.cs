using FluentAssertions;
using TeacherControl.Enums;
using TeacherControl.Extensions;
using TeacherControl.Models;

namespace TeacherControl.Tests.Models;

public class UserTests
{
    // TEST(A): Tendo parametros válidos, quando criar o usuário deve o usuário com sucesso.
    //Name: GIVEN_WHEN_THEN - (Dado..._Quando..._Entao...)
    //Dado[Contexto]_Quando[Acao]_Entao[ResultadoEsperado]
    [Fact]
    public void DadoParametrosValidos_QuandoCriarUsuario_EntaoDeveCriarComSucesso()
    {
        //Arrange
        var email = "test@gmail.com";
        var password = "password";
        var userName = "test";
        var roleId = Guid.NewGuid();

        //Act - EXECUTA
        var user = new User(email, password, userName, roleId);

        //Assert - FluentAssertion
        user.Should().NotBeNull();
        user.Email.Should().Be(email);
        user.Password.Should().Be(password);
        user.Name.Should().Be(userName);
        user.RoleId.Should().Be(roleId);
        user.Lessons.Should().BeEmpty();
        user.Status.Should().Be(UserStatus.Active);
    }

    // TEST(B): Quando tiver o email vazio, ao criar o usuário deve "estourar" uma exception.]
    [Fact]
    public void DadoEmailVazio_QuandoCriarUsuario_EntaoDeveLancarDomainException()
    {
        //Arrange
        var emailInvalid = "";
        var password = "password";
        var userName = "test";
        var roleId = Guid.NewGuid();
        
        //ACT
        var user = () => new User(emailInvalid, password, userName, roleId);
        
        //Assert
        user.Should().Throw<DomainException>().Which.Errors.Should().Contain("Email cannot be empty");
    }
    
    // TEST(C): Dado um usuário válido, quando valido deve retorna "TRUE".
    [Fact]
    public void DadoUsuarioValido_QuandoVerificarStatus_EntaoDeveSerAtivo()
    {
        // Arrange
        var email = "test@gmail.com";
        var password = "password";
        var userName = "test";
        var roleId = Guid.NewGuid();

        // Act
        var user = new User(email, password, userName, roleId);

        // Assert
        user.Should().NotBeNull();
        user.Email.Should().Be(email);
        user.Password.Should().Be(password);
        user.Name.Should().Be(userName);
        user.RoleId.Should().Be(roleId);
        user.Lessons.Should().BeEmpty();
        user.Status.Should().Be(UserStatus.Active);
    }

    // TEST(D): Nome vazio
    [Fact]
    public void DadoNomeVazio_QuandoCriarUsuario_EntaoDeveLancarDomainException()
    {
        var user = () => new User("test@gmail.com", "password", "", Guid.NewGuid());

        user.Should().Throw<DomainException>().Which.Errors.Should().Contain("Name cannot be empty");
    }

    // TEST(E): RoleId inválido
    [Fact]
    public void DadoRoleIdInvalido_QuandoCriarUsuario_EntaoDeveLancarDomainException()
    {
        var user = () => new User("test@gmail.com", "password", "test", Guid.Empty);

        user.Should().Throw<DomainException>().Which.Errors.Should().Contain("RoleId is invalid");
    }
}