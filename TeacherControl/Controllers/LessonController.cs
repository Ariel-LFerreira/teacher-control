using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Mapper;
using TeacherControl.Models;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Controllers;

[Authorize(Roles="Manager")]
[ApiController]
[Route("[controller]")]
public class LessonController(ILessonService lessonService) : ControllerBase
{
    [HttpPost("/AddLesson")]
    public async Task<ActionResult<LessonResponseDto>> Post([FromBody] LessonRequestDto lessonRequestDto)
    {
        var lessonResponse = await lessonService.Add(lessonRequestDto);
        
        return Ok(lessonResponse);
    }
    
    [HttpPut("updateLesson/{id}")]
    public async Task<ActionResult<LessonResponseDto>> Put(Guid id, [FromBody] LessonRequestDto lessonRequestDto)
    {
        var lessonUpdate = await lessonService.Update(id, lessonRequestDto);

        return Ok(lessonUpdate);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await lessonService.Remove(id);
        return NoContent();

    }
    
    [HttpGet]
    public async Task<ActionResult<List<LessonResponseDto>>> GetAll()
    {
        var listLessons = await lessonService.GetAll();
        
        return Ok(listLessons);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<LessonResponseDto>> GetById(Guid id)
    {
        var lesson = await lessonService.GetById(id);

        return Ok(lesson);
    }
    
}