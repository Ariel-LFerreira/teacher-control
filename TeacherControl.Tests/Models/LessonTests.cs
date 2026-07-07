using FluentAssertions;
using TeacherControl.Enums;
using TeacherControl.Extensions;
using TeacherControl.Models;

namespace TeacherControl.Tests.Models;

public class LessonTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoCriarLesson_EntaoDeveCriarComSucesso()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now);
        var title = "Aula 1";
        var description = "Introdução";
        var userId = Guid.NewGuid();

        // Act
        var lesson = new Lesson(date, title, description, userId);

        // Assert
        lesson.Should().NotBeNull();
        lesson.LessonDate.Should().Be(date);
        lesson.Title.Should().Be(title);
        lesson.Description.Should().Be(description);
        lesson.UserId.Should().Be(userId);
    }

    [Fact]
    public void DadoDataInvalida_QuandoCriarLesson_EntaoDeveLancarDomainException()
    {
        // Arrange
        var date = default(DateOnly);
        var title = "Aula";
        var description = "Desc";
        var userId = Guid.NewGuid();

        // Act
        var lesson = () => new Lesson(date, title, description, userId);

        // Assert
        lesson.Should().Throw<DomainException>().Which.Errors.Should().Contain("Lesson Date is invalid");
    }

    [Fact]
    public void DadoTituloVazio_QuandoCriarLesson_EntaoDeveLancarDomainException()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var title = "";
        var description = "Desc";
        var userId = Guid.NewGuid();
        
        var lesson = () => new Lesson(date, title, description, userId);

        lesson.Should().Throw<DomainException>().Which.Errors.Should().Contain("Title cannot be empty");
    }
}
