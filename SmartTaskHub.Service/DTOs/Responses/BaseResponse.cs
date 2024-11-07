namespace SmartTaskHub.Service.DTOs.Responses;

public class BaseResponse<T>
{
    /// <summary>
    /// Code
    /// </summary>
    public int Code { get; set; }
    
    /// <summary>
    /// Message
    /// </summary>
    public string? Message { get; set; }
    
    /// <summary>
    /// Data
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// Details
    /// </summary>
    public string? Details { get; set; } 
}