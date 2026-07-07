using FluentAssertions;
using TeacherControl.Models;

namespace TeacherControl.Tests.Models;

public class BaseModelTests
{
    private class FakeModel : TeacherControl.Models.Base.BaseModel
    {
        public override bool Validate() => true;
    }
    
    [Fact]
    public void DadoNovoModelo_QuandoInstanciar_EntaoDeveGerarId()
    {
        var model = new FakeModel();

        model.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void DadoModelo_QuandoAlterarId_EntaoDeveAtualizarId()
    {
        var model = new FakeModel();
        var newId = Guid.NewGuid();

        model.ChangeId(newId);

        model.Id.Should().Be(newId);
    }
}