using FluentAssertions;
using TeacherControl.DTOs.Response;

namespace TeacherControl.Tests.DTOs.Response;

public class RoleResponseDtoTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoAtribuirValoresAoRoleResponseDto_EntaoDeveArmazenarCorretamente()
    {
        // Arrange
        var name = "Admin";
        var description = "Perfil com acesso total ao sistema";

        // Act
        var dto = new RoleResponseDto
        {
            Name = name,
            Description = description
        };

        // Assert
        dto.Should().NotBeNull();
        dto.Name.Should().Be(name);
        dto.Description.Should().Be(description);
    }

    [Fact]
    public void DadoRoleResponseDtoVazio_QuandoInstanciar_EntaoDevePossuirValoresPadrao()
    {
        // Act
        var dto = new RoleResponseDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Name.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
    }

    [Fact]
    public void DadoAlteracaoDePropriedades_QuandoModificarValores_EntaoDeveRefletirAsAlteracoes()
    {
        // Arrange
        var dto = new RoleResponseDto();

        var newName = "Professor";
        var newDescription = "Responsável por ministrar aulas";

        // Act
        dto.Name = newName;
        dto.Description = newDescription;

        // Assert
        dto.Name.Should().Be(newName);
        dto.Description.Should().Be(newDescription);
    }
}