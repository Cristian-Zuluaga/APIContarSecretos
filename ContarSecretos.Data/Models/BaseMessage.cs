using System.Net;

public class BaseMessage<T>
where T : class
{
    public string Message {get;set;} = string.Empty;
    public HttpStatusCode StatusCode {get;set;}
    public List<T> ResponseElements {get;set;} = new List<T>();
}