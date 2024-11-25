namespace Authorization.Application.Responses;

public class CustomSucessResponce<T>:BaseSucessResponse
{
    public T Data { get; set; }

    public CustomSucessResponce(T? data)
    {
        Data = data;
    }
}