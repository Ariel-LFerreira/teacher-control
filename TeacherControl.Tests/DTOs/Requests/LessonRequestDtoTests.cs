using FluentAssertions;
using TeacherControl.DTOs.Requests;

namespace TeacherControl.Tests.DTOs.Requests;

public class LessonRequestDtoTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoAtribuirValoresAoLessonRequestDto_EntaoDeveArmazenarCorretamente()
    {
        // Arrange
        var date        = DateOnly.FromDateTime(DateTime.Now);
        var title       = "Aula de Matemática";
        var description = "Introdução a álgebra";
        var userId      = Guid.NewGuid();

        // Act
        var dto = new LessonRequestDto
        {
            LessonDate = date,
            Title = title,
            Description = description,
            UserId = userId
        };

        // Assert
        dto.Should().NotBeNull();
        dto.LessonDate.Should().Be(date);
        dto.Title.Should().Be(title);
        dto.Description.Should().Be(description);
        dto.UserId.Should().Be(userId);
    }
    
    [Fact]
    public void DadoLessonRequestDtoVazio_QuandoInstanciar_EntaoDevePossuirValoresPadrao()
    {
        // ARRANGE
        // POR PADRÂO TEM QUE COMEÇAR VÁZIO.
        
        // Act
        var dto = new LessonRequestDto();

        // Assert
        dto.Title.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
        dto.UserId.Should().Be(Guid.Empty);
    }


}