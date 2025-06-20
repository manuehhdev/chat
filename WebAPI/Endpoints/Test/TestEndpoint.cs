using Microsoft.AspNetCore.Http.HttpResults;

namespace WebAPI.Endpoints.Test;

public class TestEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/test", Handle)
        .RequireAuthorization();

    public static Ok Handle()
    {
        return TypedResults.Ok();
    }
  
}