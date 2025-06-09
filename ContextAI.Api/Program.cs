using ContextAI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Register MVC controllers for HTTP API support
builder.Services.AddControllers();

// Register Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enforce HTTPS redirection
app.UseHttpsRedirection();

// Enforce authorization middleware (optional: configure policies)
app.UseAuthorization();

// Map HTTP controllers (API endpoints)
app.MapControllers();

await app.RunAsync();
