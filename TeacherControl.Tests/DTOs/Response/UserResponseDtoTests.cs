using FluentAssertions;
using TeacherControl.DTOs.Response;

namespace TeacherControl.Tests.DTOs.Response;

public class UserResponseDtoTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoAtribuirValoresAoUserResponseDto_EntaoDeveArmazenarCorretamente()
    {
    // Arrange
    var email = "[user@gmail.com](mailto:user@gmail.com)";
    var name = "Usuário";
    var status = "Active";

        var role = new RoleResponseDto
        {
            Name = "Admin",
            Description = "Acesso total"
        };

        var lessons = new List<LessonResponseDto>
        {
            new LessonResponseDto
            {
                Title = "Aula 1",
                Description = "Descrição 1"
            },
            new LessonResponseDto
            {
                Title = "Aula 2",
                Description = "Descrição 2"
            }
        };

        // Act
        var dto = new UserResponseDto
        {
            Email = email,
            Name = name,
            Status = status,
            Role = role,
            Lessons = lessons
        };

        // Assert
        dto.Should().NotBeNull();
        dto.Email.Should().Be(email);
        dto.Name.Should().Be(name);
        dto.Status.Should().Be(status);

        dto.Role.Should().NotBeNull();
        dto.Role.Should().Be(role);

        dto.Lessons.Should().NotBeNull();
        dto.Lessons.Should().HaveCount(2);
    }

    [Fact]
    public void DadoUserResponseDtoVazio_QuandoInstanciar_EntaoDevePossuirValoresPadrao()
    {
        // Act
        var dto = new UserResponseDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Email.Should().BeNull();
        dto.Name.Should().BeNull();
        dto.Status.Should().BeNull();
        dto.Role.Should().BeNull();
        dto.Lessons.Should().BeNull();
    }

    [Fact]
    public void DadoPropriedadesNulas_QuandoNaoAtribuirValores_EntaoDevePermitirValoresNulos()
    {
        // Arrange
        var dto = new UserResponseDto
        {
            Email = null,
            Name = null,
            Status = null,
            Role = null,
            Lessons = null
        };

        // Assert
        dto.Email.Should().BeNull();
        dto.Name.Should().BeNull();
        dto.Status.Should().BeNull();
        dto.Role.Should().BeNull();
        dto.Lessons.Should().BeNull();
    }

    [Fact]
    public void DadoAlteracaoDePropriedades_QuandoModificarValores_EntaoDeveRefletirAsAlteracoes()
    {
        // Arrange
        var dto = new UserResponseDto();

        var newEmail = "novo@gmail.com";
        var newName = "Novo Nome";
        var newStatus = "Inactive";

        var newRole = new RoleResponseDto
        {
            Name = "Professor",
            Description = "Ministra aulas"
        };

        var newLessons = new List<LessonResponseDto>
        {
            new LessonResponseDto
            {
                Title = "Nova Aula",
                Description = "Nova descrição"
            }
        };

        // Act
        dto.Email = newEmail;
        dto.Name = newName;
        dto.Status = newStatus;
        dto.Role = newRole;
        dto.Lessons = newLessons;

        // Assert
        dto.Email.Should().Be(newEmail);
        dto.Name.Should().Be(newName);
        dto.Status.Should().Be(newStatus);
        dto.Role.Should().Be(newRole);
        dto.Lessons.Should().HaveCount(1);
    }

}
