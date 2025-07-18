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

// Asset Versioning Services - Temporarily disabled
// builder.Services.Configure<AssetVersioningOptions>(
//     builder.Configuration.GetSection(AssetVersioningOptions.SectionName));
// builder.Services.AddScoped<IAssetVersioningService, AssetVersioningService>();
// builder.Services.AddScoped<VersionedAssetTagHelper>();
// builder.Services.AddHostedService<FileWatcherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

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
});

app.Run();
