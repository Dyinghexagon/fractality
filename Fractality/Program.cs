using Fractality;
using Fractality.Context;
using Fractality.Repositories;
using Fractality.Services.UserServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.PropertyNamingPolicy = null;
                     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                 });

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});

var IsDevelopment = true;

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "SimpleTalk.AuthCookieAspNetCore";
    options.LoginPath = "/api/auth/login";
    options.LogoutPath = "/api/auth/logout";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = IsDevelopment ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.None;
    options.Secure = IsDevelopment ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
});

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile));
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services.AddScoped<IDbRepository, DbRepository>();

builder.Services.AddTransient<IUsersServices, UserServices>();

IFileProvider physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
builder.Services.AddSingleton<IFileProvider>(physicalProvider);

builder.Services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCookiePolicy();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

var logger = app.Services.GetService<ILogger<Program>>();

app.Use(async (context, next) =>
{
    var principal = context.User as ClaimsPrincipal;
    var accessToken = principal.Claims.FirstOrDefault(c => c.Type == "access_token");

    if (accessToken != null)
    {
        logger?.LogDebug(accessToken.Value);
    }
    await next();
});

app.Run();