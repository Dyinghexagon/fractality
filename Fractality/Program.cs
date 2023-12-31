using Fractality;
using Fractality.Context;
using Fractality.Repositories;
using Fractality.Services.UserServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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

builder.Services.AddAuthentication();
builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile));
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services.AddScoped<IDbRepository, DbRepository>();

builder.Services.AddTransient<IUsersServices, UserServices>();

IFileProvider physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
builder.Services.AddSingleton<IFileProvider>(physicalProvider);

builder.Services.AddMvc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();