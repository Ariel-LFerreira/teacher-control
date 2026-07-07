using FluentAssertions;
using TeacherControl.DTOs.Requests;

namespace TeacherControl.Tests.DTOs.Requests;

public class LoginRequestDtoTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoAtribuirValoresALoginRequestDto_EntaoDeveArmazenarCorretamente()
    {
        //Arrange
        var email = "teste@gmail.com";
        var password = "password";
        
        //ACT
        var dtoLogin = new LoginRequestDto
        {
            Email = email,
            Password = password
        };
        
        //ASSERT
        dtoLogin.Should().NotBeNull();
        dtoLogin.Email.Should().Be(email);
        dtoLogin.Password.Should().Be(password);
    }
    
    [Fact]
    public void DadoLoginRequestDtoVazio_QuandoInstanciar_EntaoDevePossuirValoresPadrao()
    {
        // ARRANGE
        // POR PADRÂO TEM QUE COMEÇAR VÁZIO.
        
        // Act
        var dto = new LoginRequestDto();

        // Assert
        dto.Email.Should().BeEmpty();
        dto.Password.Should().BeEmpty();
    }
}