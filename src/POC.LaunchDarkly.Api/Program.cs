using POC.LaunchDarkly.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();
app.Configure();

app.Run();
