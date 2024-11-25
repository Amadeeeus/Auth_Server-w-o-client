namespace Authorization.Application.Responses;

public class BaseSucessResponse
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }

    public BaseSucessResponse()
    {
        Success = true;
        StatusCode = StatusCodes.Status200OK;
    }
}