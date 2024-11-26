using Authorization.Core.Filters;
using Authorization.Infrastructure.DataAccess;
using Authorization.Infrastructure.Extensions;
using Authorization.Infrastructure.Middleware;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;


DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationServices();
builder.Services.AddServices();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION")));
builder.Services.AddMediatr();
builder.Services.AddControllers(opt =>
    {
        opt.Filters.Add<ExceptionHandleFilter>();
    }
);
builder.Services.AddApiAuthentification();
builder.Services.AddCustomCors();
builder.Services.AddStackExchangeRedisCache(configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.UseMiddleware<AuthMiddleware>();
app.Run();

