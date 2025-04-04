using POC.LaunchDarkly.Api.Endpoints;
using POC.LaunchDarkly.Api.Extensions;
using POC.LaunchDarkly.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.UseMiddleware<ExceptionMiddleware>();
app.MapEmprestimoEndpoints();

app.Run();

