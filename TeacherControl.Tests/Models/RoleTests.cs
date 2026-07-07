using FluentAssertions;
using TeacherControl.Extensions;
using TeacherControl.Models;

namespace TeacherControl.Tests.Models;

public class RoleTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoCriarRole_EntaoDeveCriarComSucesso()
    {
        // Arrange
        var name = "Admin";
        var description = "Acesso total";

        // Act
        var role = new Role(name, description);

        // Assert
        role.Should().NotBeNull();
        role.Name.Should().Be(name);
        role.Description.Should().Be(description);
    }

    [Fact]
    public void DadoNomeVazio_QuandoCriarRole_EntaoDeveLancarDomainException()
    {
        // Arrange
        var name = "";
        var description = "Descrição válida";

        // Act
        var role = () => new Role(name, description);

        // Assert
        role.Should().Throw<DomainException>().Which.Errors.Should().Contain("Name cannot be empty");
    }

    [Fact]
    public void DadoDescricaoVazia_QuandoCriarRole_EntaoDeveLancarDomainException()
    {
        // Arrange
        var name = "Admin";
        var description = "";

        // Act
        var role = () => new Role(name, description);

        // Assert
        role.Should().Throw<DomainException>().Which.Errors.Should().Contain("Description is invalid");
    }
}
