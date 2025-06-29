using FieldShield.SeawedFileAPI;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var seawedFileApiUrl = builder.Configuration.GetValue<string>("FileServerAPI:BaseUrl") ?? string.Empty;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHealthChecksUI(setup =>
    {
        setup.SetApiMaxActiveRequests(100);
        setup.AddHealthCheckEndpoint("Seawed File API", "/api/health");
        setup.DisableDatabaseMigrations();
    })
    .AddInMemoryStorage();

builder.Services
    .AddHealthChecks()
    .AddCheck<ExternalApiHealthCheck>(Constants.SeawedFileApiName,
        tags: new[] { "ready" },
        failureStatus: HealthStatus.Degraded,
        timeout:TimeSpan.FromSeconds(10)
    );

builder.Services.AddHttpClient(Constants.SeawedFileApiName, client =>
{
    client.BaseAddress = new Uri(seawedFileApiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/api/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHealthChecksUI(options =>
{
    options.UIPath = "/healthcheck-ui";
    options.AddCustomStylesheet("./Common/Custom.css");

});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
