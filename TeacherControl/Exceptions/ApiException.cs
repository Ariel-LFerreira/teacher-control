namespace TeacherControl.Exceptions;

public class ApiException : Exception
{
    public ApiException(){}
    
    public ApiException(string message, int statusCode = 500, string? details = null) : base(message)
    {
        StatusCode = statusCode;
        Details = details;
    }
    
    public int StatusCode { get; } 
    public string? Details { get; }
    

}