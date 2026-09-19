var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRepresentationLayer(builder.Configuration);
builder.Services.AddApplication(builder.Configuration, builder.Environment);

var app = builder.Build();

// use services

using var scope = app.Services.CreateScope();

await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

app.UseRepresentation();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
