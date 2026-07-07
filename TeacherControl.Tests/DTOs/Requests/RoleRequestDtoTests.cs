using FluentAssertions;
using TeacherControl.DTOs.Requests;
using TeacherControl.Models;

namespace TeacherControl.Tests.DTOs.Requests;

public class RoleRequestDtoTests
{
    [Fact]
    public void DadoParametrosValidos_QuandoAtribuirValoresAoRoleRequestDto_EntaoDeveArmazenarCorretamente()
    {
        // Arrange
        var name = "NameRoleTeste";
        var description = "Descrição Role Testes";
        
        //ACT
        var dtoRole = new RoleRequestDto
        {
            Name = name,
            Description =  description
        };
        
        //ASSERT
        dtoRole.Should().NotBeNull();
        dtoRole.Name.Should().Be(name);
        dtoRole.Description.Should().Be(description);
    }
    
    [Fact]
    public void DadoRoleRequestDtoVazio_QuandoInstanciar_EntaoDevePossuirValoresPadrao()
    {
        // ARRANGE
        // POR PADRÂO TEM QUE COMEÇAR VÁZIO.
        
        // Act
        var dto = new RoleRequestDto();

        // Assert
        dto.Name.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
    }
}