using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FieldShield.SeawedFileAPI;

public class ExternalApiHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;

    public ExternalApiHealthCheck(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(Constants.SeawedFileApiName);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            //using a default file as health check endpoint
            var response = await _httpClient
                .GetAsync($"{Constants.DefaultHealthCheckFileName}?metadata={true}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("Seawed File API is healthy.");
            }
            else
            {
                return HealthCheckResult.Unhealthy("Seawed File API is unhealthy.", null, new Dictionary<string, object>
                {
                    { "StatusCode", response.StatusCode }
                });
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Seawed File API is unhealthy.", ex);
        }
    }
}
