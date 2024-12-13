namespace Business.Models;

public class ResponseResult<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; } 
}
