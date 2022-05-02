using KC.Models;
using KC.WebApi.Repository;
using KC.WebApi.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();


builder.Host.UseSerilog((_, lc) => lc
    .WriteTo.Console());

// Add services to the container.
builder.Services.AddSingleton<IWeatherForecastRepository, WeatherForecastRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsPolicyBuilder => 
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var configuration = builder.Configuration;

var authority = configuration.GetValue<string>("Options:Authority"); 
var audience = configuration.GetValue<string>("Options:ClientId");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer("Bearer", opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.Authority = authority;
        opt.Audience = audience;
        opt.SaveToken = true;
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudiences = new string[] { audience, "realm-management", "account" }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.NeedsWeatherForecast,  Policies.NeedsWeatherForecastPolicy());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();