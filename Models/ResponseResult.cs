using Microsoft.Extensions.Primitives;

public class ResponseResult<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    
    public string Message { get; set; }
    public T Data { get; set; }

   
    public ResponseResult(int statusCode, bool success, string message, T data)
    {
        StatusCode = statusCode;
        Success = success;
        Message = message;
        Data = data;
    }

}
