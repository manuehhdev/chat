using WebAPI.Endpoints.Authentication;
using WebAPI.Endpoints.Test;
using WebAPI.Endpoints.User;
using WebAPI.Filters;

namespace WebAPI.Endpoints;

public static class EndpointsMapping
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app = app.MapGroup("api");
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        //app.MapEndpoint<TestEndpoint>();
    }

    static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapEndpoint<Register>();
        group.MapEndpoint<Login>();
        group.MapEndpoint<Refresh>();

    }

    static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/user");

        group.MapEndpoint<UserInfo>();
        group.MapEndpoint<ChangeProfilePic>();
    }

    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder
            .AddEndpointFilter<ValidationFilter<TRequest>>()
            .ProducesValidationProblem();
    }
    
    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app); 
        return app;
    }
}