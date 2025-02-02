using System.Net;
using Microsoft.AspNetCore.Http;

namespace FoodQualityAnalysis.Common.Utilities.Error;

public class ApiException: Exception
{
    public HttpStatusCode StatusCode { get; }
    
    public ApiException(HttpStatusCode statusCode, string message,Exception? exception = null ) : base(message,exception)
    {
        StatusCode = statusCode;
    }
    
}