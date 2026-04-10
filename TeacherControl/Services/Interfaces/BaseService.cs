using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;

namespace TeacherControl.Services.Interfaces;

public  abstract class BaseService<TEntity, TResquestDto, TResponseDto> : IBaseService<TResquestDto, TResponseDto> where TEntity : BaseModel
{

    protected readonly IBaseRepository<TEntity> _repository;

    protected BaseService(IBaseRepository<TEntity> repository)
    {
        _repository = repository;
    }

    protected abstract TResponseDto ToResponse(TEntity entity);
    protected abstract TEntity ToEntity(TResquestDto resquestDto);

    public async Task<TResponseDto> Add(TResquestDto requestDto)
    {
        var entity = ToEntity(requestDto);
        
        await _repository.Add(entity);

        return ToResponse(entity);
    }

    public async Task<TResponseDto> Update(Guid id, TResquestDto request)
    {
        var entity = await _repository.GetById(id);

        if (entity is null)
            throw new Exception("Entity not found!");
        
        var updated = ToEntity(request);

        updated.ChangeID(entity.Id);

        await _repository.Update(updated);

        return ToResponse(updated);
    }

    public async Task Remove(Guid id)
    {
        var entity = await _repository.GetById(id);

        if (entity is null)
            throw new Exception("Entity not found!");

        await _repository.Remove(entity.Id);
    }

    public async Task<TResponseDto?> GetById(Guid id)
    {
        var entity = await _repository.GetById(id);

        if (entity is null)
            throw new Exception("Item not found!");

        return ToResponse(entity);
    }

    public async Task<List<TResponseDto>> GetAll()
    {
        var listEntity = await _repository.GetAll();

        if (listEntity == null)
            throw new Exception("List not found!");

        return listEntity.Select(ToResponse).ToList();
    }
    
    
    
}