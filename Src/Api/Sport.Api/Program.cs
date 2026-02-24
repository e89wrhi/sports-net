using Api.Extensions;
using Event.Extensions.Infrastructure;
using Identity.Extensions.Infrastructure;
using Intelligence.Extensions;
using Match.Extensions.Infrastructure;
using Sport.Common.Web;
using Vote.Extensions.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedInfrastructure();

// Automated discovery for IModule implementations (Identity and ChatBot)
builder.AddModules();

var app = builder.Build();

// ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-7.0#routing-basics
app.UseAuthentication();
app.UseAuthorization();

// Automatically configures middleware for discovered modules (Identity and ChatBot)
app.UseModules();

app.UserSharedInfrastructure();
app.MapMinimalEndpoints();

app.Run();

namespace Api
{
    public partial class Program
    {
    }
}