using BloggerProUI.Business.Interfaces;
using BloggerProUI.Business.Services;
using BloggerProUI.Web.Extensions;
using BloggerProUI.Web.Models.Configuration;
using BloggerProUI.Web.Services;
using BloggerProUI.Web.TagHelpers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddConfiguredHttpClients(builder.Configuration);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.LogoutPath = "/Auth/Logout";
            options.AccessDeniedPath = "/Auth/Login";
        });
builder.Services.AddTransient<BloggerProUI.Business.Handlers.AuthTokenHandler>();
builder.Services.AddScoped<BloggerProUI.Web.Services.SeoConfigurationService>();

// Asset Versioning Services - Cache Busting
builder.Services.Configure<AssetVersioningOptions>(
    builder.Configuration.GetSection(AssetVersioningOptions.SectionName));
builder.Services.AddScoped<IAssetVersioningService, AssetVersioningService>();
builder.Services.AddScoped<VersionedAssetTagHelper>();
builder.Services.AddHostedService<FileWatcherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// ULTRA AGRESIF CACHE DEVRE DIŞI - Her request için
app.Use(async (context, next) =>
{
    // Response headers - cache'i tamamen devre dışı bırak
    context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate, private";
    context.Response.Headers.Pragma = "no-cache";
    context.Response.Headers.Expires = "Thu, 01 Jan 1970 00:00:00 GMT";
    context.Response.Headers.ETag = $"\"{Guid.NewGuid()}\"";
    context.Response.Headers.LastModified = DateTime.UtcNow.ToString("R");
    context.Response.Headers["X-Cache-Control"] = "no-cache";
    
    // Request headers - cache'li request'leri engelle
    context.Request.Headers.Remove("If-Modified-Since");
    context.Request.Headers.Remove("If-None-Match");
    
    await next();
});

// Static files - CACHE TAMAMEN KAPALI
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        var headers = context.Context.Response.Headers;
        headers.CacheControl = "no-cache, no-store, must-revalidate";
        headers.Pragma = "no-cache";
        headers.Expires = "0";
        headers.ETag = $"\"{DateTime.Now.Ticks}\"";
        headers.LastModified = DateTime.UtcNow.ToString("R");
    }
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// MapStaticAssets kaldırıldı - cache sorununa neden oluyor

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=AdminDashboard}/{action=Index}/{id?}");

    // Attribute routing için
    endpoints.MapControllers();

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
        
    // SignalR Hub mapping will connect to API's ChatHub
    // Note: ChatHub is hosted on API side at https://localhost:7028/chathub
});

app.Run();
