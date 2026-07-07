using FluentAssertions;
using TeacherControl.DTOs.Response;

namespace TeacherControl.Tests.DTOs.Response;

public class UserResponseSummaryDtoTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoAtribuirValoresAoUserResponseSummaryDto_EntaoDeveArmazenarCorretamente()
    {
        // Arrange
        var email = "[user@gmail.com](mailto:user@gmail.com)";
        var name = "Usuário";
        var status = "Active";

        // Act
        var dto = new UserResponseSummaryDto
        {
            Email = email,
            Name = name,
            Status = status
        };

        // Assert
        dto.Should().NotBeNull();
        dto.Email.Should().Be(email);
        dto.Name.Should().Be(name);
        dto.Status.Should().Be(status);
    }

    [Fact]
    public void DadoUserResponseSummaryDtoVazio_QuandoInstanciar_EntaoDevePossuirValoresPadrao()
    {
        // Act
        var dto = new UserResponseSummaryDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Email.Should().BeNull();
        dto.Name.Should().BeNull();
        dto.Status.Should().BeNull();
    }

    [Fact]
    public void DadoPropriedadesNulas_QuandoNaoAtribuirValores_EntaoDevePermitirValoresNulos()
    {
        // Arrange
        var dto = new UserResponseSummaryDto
        {
            Email = null,
            Name = null,
            Status = null
        };

        // Assert
        dto.Email.Should().BeNull();
        dto.Name.Should().BeNull();
        dto.Status.Should().BeNull();
    }

    [Fact]
    public void DadoAlteracaoDePropriedades_QuandoModificarValores_EntaoDeveRefletirAsAlteracoes()
    {
        // Arrange
        var dto = new UserResponseSummaryDto();

        var newEmail = "novo@gmail.com";
        var newName = "Novo Nome";
        var newStatus = "Inactive";

        // Act
        dto.Email = newEmail;
        dto.Name = newName;
        dto.Status = newStatus;

        // Assert
        dto.Email.Should().Be(newEmail);
        dto.Name.Should().Be(newName);
        dto.Status.Should().Be(newStatus);
    }
}