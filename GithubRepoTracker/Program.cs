using GithubRepoTracker.Interfaces;
using GithubRepoTracker.Services;
using GithubRepoTracker.ViewModels;
using GithubRepoTracker.Models;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<RepoInterface, RepoService>();
builder.Services.AddScoped<TopicInterface, TopicService>();
builder.Services.AddScoped<LanguageInterface, LanguageService>();
builder.Services.AddScoped<ApiAuthInterface, ApiAuthService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<RepoListViewModel, RepoListViewModel>();
// Configure caching

builder.Services.AddMemoryCache();
builder.Services.Configure<MemoryCacheOptions>(options =>
{
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(30);
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
