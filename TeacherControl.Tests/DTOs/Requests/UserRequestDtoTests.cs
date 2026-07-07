using FluentAssertions;
using TeacherControl.DTOs.Requests;

namespace TeacherControl.Tests.DTOs.Requests;

public class UserRequestDtoTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoAtribuirValoresAoUserRequestDto_EntaoDeveArmazenarCorretamente()
    {
        // Arrange
        var email = "test@gmail.com";
        var password = "123456";
        var name = "Usuário Teste";
        var roleId = Guid.NewGuid();

        // Act
        var dto = new UserRequestDto
        {
            Email = email,
            Password = password,
            Name = name,
            RoleId = roleId
        };

        // Assert
        dto.Should().NotBeNull();
        dto.Email.Should().Be(email);
        dto.Password.Should().Be(password);
        dto.Name.Should().Be(name);
        dto.RoleId.Should().Be(roleId);
    }

    [Fact]
    public void DadoUserRequestDtoVazio_QuandoInstanciar_EntaoDevePossuirValoresPadrao()
    {
        // Act
        var dto = new UserRequestDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Email.Should().BeEmpty();
        dto.Password.Should().BeEmpty();
        dto.Name.Should().BeEmpty();
        dto.RoleId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void DadoAlteracaoDePropriedades_QuandoModificarValores_EntaoDeveRefletirAsAlteracoes()
    {
        // Arrange
        var dto = new UserRequestDto();

        var newEmail = "novo@gmail.com";
        var newPassword = "novaSenha";
        var newName = "Novo Nome";
        var newRoleId = Guid.NewGuid();

        // Act
        dto.Email = newEmail;
        dto.Password = newPassword;
        dto.Name = newName;
        dto.RoleId = newRoleId;

        // Assert
        dto.Email.Should().Be(newEmail);
        dto.Password.Should().Be(newPassword);
        dto.Name.Should().Be(newName);
        dto.RoleId.Should().Be(newRoleId);
    }

}