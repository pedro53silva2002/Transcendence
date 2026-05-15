DotNetEnv.Env.TraversePath().Load();

var port = Environment.GetEnvironmentVariable("BACKEND_PORT") ?? "5024";
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://+:{port}");

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapOpenApi();
app.MapHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
