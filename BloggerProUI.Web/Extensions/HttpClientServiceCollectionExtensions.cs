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
        .AddHttpMessageHandler<AuthTokenHandler>();


        services.AddHttpClient<ICategoryApiService, CategoryApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<ITagApiService, TagApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IPostApiService, PostApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>(); 

        services.AddHttpClient<ICommentApiService, CommentApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();
        
        services.AddHttpClient<IImageApiService, ImageApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IUserApiService, UserApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        });

        services.AddHttpClient<IContactApiService, ContactApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IAboutUsApiService, AboutUsApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<ITeamMemberApiService, TeamMemberApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IFooterApiService, FooterApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IBookmarkApiService, BookmarkApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<UserDashboardApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IUserProfileApiService, UserProfileApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IChatApiService, ChatApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IUserSearchApiService, UserSearchApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<IPresenceApiService, PresenceApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        services.AddHttpClient<INotificationApiService, NotificationApiService>(client =>
        {
            client.BaseAddress = baseAddress;
        })
        .AddHttpMessageHandler<AuthTokenHandler>();

        return services;
    }
}
