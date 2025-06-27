
using FieldShield.ApplicationCore.Interfaces.Persistence;
using FieldShield.Infrastructure.Persistence;
using FieldShield.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FieldShield.Infrastructure;
public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<FileShieldDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("FileShieldDb"));
        });

        // Add Redis db
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisDb");
        });

        // Add Repositories
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFolderRepository, FolderRepository>();

        // Add HttpClient for SeawedFileAPI
        //services.AddHttpClient("SeawedFileAPI", client =>
        //{
        //    client.BaseAddress = new Uri(configuration["SeawedFileAPI:BaseUrl"]);
        //    client.Timeout = TimeSpan.FromMinutes(5);
        //});

        return services;
    }
}
