using FluentAssertions;
using TeacherControl.DTOs.Response;

namespace TeacherControl.Tests.DTOs.Response;

public class LoginResponseDtoTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoAtribuirValoresAoLoginResponseDto_EntaoDeveArmazenarCorretamente()
    {
        // Arrange
        var id = Guid.NewGuid();
        var token = "jwt-token";
        var email = "[test@gmail.com](mailto:test@gmail.com)";
        var name = "Usuário Teste";

        var role = new RoleResponseDto
        {
            Description = "DESCRIPTION ROLE",
            Name = "Admin"
        };

        // Act
        var dto = new LoginResponseDto
        {
            Id = id,
            Token = token,
            Email = email,
            Name = name,
            role = role
        };

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(id);
        dto.Token.Should().Be(token);
        dto.Email.Should().Be(email);
        dto.Name.Should().Be(name);
        dto.role.Should().NotBeNull();
        dto.role.Should().Be(role);
    }

    [Fact]
    public void DadoLoginResponseDtoVazio_QuandoInstanciar_EntaoDevePossuirValoresPadrao()
    {
        // Act
        var dto = new LoginResponseDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(Guid.Empty);
        dto.Token.Should().BeEmpty();
        dto.Email.Should().BeEmpty();
        dto.Name.Should().BeEmpty();
        dto.role.Should().BeNull();
    }

    [Fact]
    public void DadoRoleNula_QuandoNaoAtribuirRole_EntaoDevePermitirValorNulo()
    {
        // Arrange
        var dto = new LoginResponseDto
        {
            Id = Guid.NewGuid(),
            Token = "token",
            Email = "email@gmail.com",
            Name = "Nome",
            role = null
        };

        // Assert
        dto.role.Should().BeNull();
    }

    [Fact]
    public void DadoAlteracaoDePropriedades_QuandoModificarValores_EntaoDeveRefletirAsAlteracoes()
    {
        // Arrange
        var dto = new LoginResponseDto();

        var newId = Guid.NewGuid();
        var newToken = "novo-token";
        var newEmail = "novo@gmail.com";
        var newName = "Novo Nome";

        var newRole = new RoleResponseDto
        {
            Description = "DeEscription",
            Name = "Professor"
        };

        // Act
        dto.Id = newId;
        dto.Token = newToken;
        dto.Email = newEmail;
        dto.Name = newName;
        dto.role = newRole;

        // Assert
        dto.Id.Should().Be(newId);
        dto.Token.Should().Be(newToken);
        dto.Email.Should().Be(newEmail);
        dto.Name.Should().Be(newName);
        dto.role.Should().Be(newRole);
    }
}
