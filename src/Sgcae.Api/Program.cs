using System.Text.Json.Serialization;
using Sgcae.Api.ErrorHandling;
using Sgcae.Application;
using Sgcae.Infrastructure;
using Sgcae.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SgcaeDb")
    ?? throw new InvalidOperationException("Connection string 'SgcaeDb' is not configured.");

builder.Services
    .AddApplication()
    .AddInfrastructure(connectionString);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

await app.Services.InitializeDatabaseAsync();

app.Run();
