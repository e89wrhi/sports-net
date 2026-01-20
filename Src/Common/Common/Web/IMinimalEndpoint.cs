using Microsoft.AspNetCore.Routing;

namespace Sport.Common.Web;

public interface IMinimalEndpoint
{
    IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder);
}