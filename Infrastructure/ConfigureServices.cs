using Flurl.Http.Configuration;
using Infrastructure.UserData;
using Microsoft.Extensions.DependencyInjection;
using Common;
using Application.Interface;
using Infrastructure.Caching;
using Flurl.Http;
using Infrastructure.Policy;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInsfrastructure(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
        services.AddTransient(typeof(IUserClient<>), typeof(UserClient<>));
        services.AddTransient<IUserEngine, UserEngine>();
        services.AddScoped<ICacheManager, CachingManager>();

        // Polly - Retry & Timeout policies configuration
        FlurlHttp.Configure(settings => settings.HttpClientFactory = new CustomPollyHttpClientFactory());

        return services;
    }
}
