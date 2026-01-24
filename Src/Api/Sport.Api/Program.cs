using Api.Extensions;
using Event.Extensions.Infrastructure;
using Identity.Extensions.Infrastructure;
using Match.Extensions.Infrastructure;
using Sport.Common.Web;
using Vote.Extensions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();

builder.AddIdentityModules();
builder.AddEventModules();
builder.AddMatchModules();
builder.AddVoteModules();

var app = builder.Build();

// ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-7.0#routing-basics
app.UseAuthentication();
app.UseAuthorization();

app.UseEventModules();
app.UseIdentityModules();
app.UseMatchModules();
app.UseVoteModules();

app.UserSharedInfrastructure();
app.MapMinimalEndpoints();

app.Run();

namespace Api
{
    public partial class Program
    {
    }
}