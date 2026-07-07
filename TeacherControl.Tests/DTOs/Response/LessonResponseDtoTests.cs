using FluentAssertions;
using TeacherControl.DTOs.Response;

namespace TeacherControl.Tests.DTOs.Response;

public class LessonResponseDtoTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoAtribuirValoresAoLessonResponseDto_EntaoDeveArmazenarCorretamente()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now);
        var title = "Aula de História";
        var description = "Revolução Francesa";
        var status = "Ativa";

        // Act
        var dto = new LessonResponseDto
        {
            LessonDate = date,
            Title = title,
            Description = description,
            Status = status
        };

        // Assert
        dto.Should().NotBeNull();
        dto.LessonDate.Should().Be(date);
        dto.Title.Should().Be(title);
        dto.Description.Should().Be(description);
        dto.Status.Should().Be(status);
    }

    [Fact]
    public void DadoLessonResponseDtoVazio_QuandoInstanciar_EntaoDevePossuirValoresPadrao()
    {
        // Act
        var dto = new LessonResponseDto();

        // Assert
        dto.Should().NotBeNull();
        dto.Title.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
        dto.Status.Should().BeNull();
    }

    [Fact]
    public void DadoStatusNulo_QuandoNaoAtribuirStatus_EntaoDevePermitirValorNulo()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now);
        var title = "Aula de Física";
        var description = "Leis de Newton";

        // Act
        var dto = new LessonResponseDto
        {
            LessonDate = date,
            Title = title,
            Description = description,
            Status = null
        };

        // Assert
        dto.Status.Should().BeNull();
    }

    [Fact]
    public void DadoAlteracaoDePropriedades_QuandoModificarValores_EntaoDeveRefletirAsAlteracoes()
    {
        // Arrange
        var dto = new LessonResponseDto();

        var newDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        var newTitle = "Nova Aula";
        var newDescription = "Descrição atualizada";
        var newStatus = "Finalizada";

        // Act
        dto.LessonDate = newDate;
        dto.Title = newTitle;
        dto.Description = newDescription;
        dto.Status = newStatus;

        // Assert
        dto.LessonDate.Should().Be(newDate);
        dto.Title.Should().Be(newTitle);
        dto.Description.Should().Be(newDescription);
        dto.Status.Should().Be(newStatus);
    }

}
