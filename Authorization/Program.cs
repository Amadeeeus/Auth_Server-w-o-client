using Authorization.Core.Filters;
using Authorization.Infrastructure.DataAccess;
using Authorization.Infrastructure.Extensions;
using Authorization.Infrastructure.Middleware;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;


DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Добавление контроллеров
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<ExceptionHandleFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationServices();
builder.Services.AddServices();
builder.Services.AddSwaggerGen();
builder.Services.AddSpaFiles();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION")));
builder.Services.AddMediatr();
builder.Services.AddApiAuthentification();
builder.Services.AddCustomCors();
builder.Services.AddStackExchangeRedisCache(configuration);
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); // Важно: вызывается перед авторизацией
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("DefaultPolicy");

app.MapControllers();

app.UseMiddleware<AuthMiddleware>();

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp/dist";
    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    spa.UseAngularCliServer(npmScript: "start");
});

app.Run();