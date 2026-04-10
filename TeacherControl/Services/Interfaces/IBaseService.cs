namespace TeacherControl.Services.Interfaces;

public interface IBaseService<TRequestDto, TResponseDto>
{
    Task<TResponseDto> Add(TRequestDto requestDto);
    
    Task<TResponseDto> Update (Guid id, TRequestDto request);
    
    Task Remove(Guid id);
    
    Task<TResponseDto?> GetById(Guid id);
    
    Task<List<TResponseDto>> GetAll();
}