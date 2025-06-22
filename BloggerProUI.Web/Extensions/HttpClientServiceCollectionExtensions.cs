using BloggerProUI.Business.Handlers;
using BloggerProUI.Business.Interfaces;
using BloggerProUI.Business.Services;
using System.Net.Http;

namespace BloggerProUI.Web.Extensions;

public static class HttpClientServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguredHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var baseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);

        services.AddTransient<AuthTokenHandler>();

        var insecureHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        services.AddHttpClient<IAdminDashboardApiService, AdminDashboardApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>()
        .ConfigurePrimaryHttpMessageHandler(() => insecureHandler);

        services.AddHttpClient<ICategoryApiService, CategoryApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>()
        .ConfigurePrimaryHttpMessageHandler(() => insecureHandler);

        services.AddHttpClient<ITagApiService, TagApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>()
        .ConfigurePrimaryHttpMessageHandler(() => insecureHandler);

        services.AddHttpClient<IPostApiService, PostApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>()
        .ConfigurePrimaryHttpMessageHandler(() => insecureHandler);

        services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .ConfigurePrimaryHttpMessageHandler(() => insecureHandler);

        return services;
    }
}
