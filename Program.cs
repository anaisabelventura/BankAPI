using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using System.Data;
using Npgsql;
using Serilog;
using System.Text;
using BankAPI.Data;
using BankAPI.Data.DBInterfaces;
using BankAPI.Business;
using BankAPI.Business.Interfaces;
using BankAPI.Core.Models;
using BankAPI.Infrastructure.Data;
//using BankAPI.Application.Business.Interfaces;
using BankAPI.Application.Business;
using BankAPI.Infrastructure.Data.DBInterfaces;
using BankAPI.Infrastructure.Authentication.Interface;
using BankAPI.Application.Business.Interfaces;
using BankAPI.Infrastructure.Authentication;

var builder = WebApplication.CreateBuilder(args);

/*builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());*/
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<postgresContext>(option => option.UseNpgsql(builder.Configuration["ConnectionStrings:postgres"]));
builder.Services.AddScoped<IDbTransaction>(s =>
{
    NpgsqlConnection connection = (NpgsqlConnection)s.GetRequiredService<IDbConnection>();
    connection.Open();
    return connection.BeginTransaction();
});

//builder.Services.AddScoped<IDbConnection>((s) => new NpgsqlConnection(builder.Configuration.GetConnectionString("BankAPIDB")));
//builder.Services.AddDbContext<BankAPIDBContext>(option => option.UseNpgsql(builder.Configuration["ConnectionStrings:postgres"]));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitofWork, UnitOfWork>();
builder.Services.AddScoped<IAccountDB, AccountDB>();
builder.Services.AddScoped<IUserDB, UserDB>();
builder.Services.AddScoped<ITransferDB, TransferDB>();
builder.Services.AddScoped<IBaseDB<Transfer>, TransferDB>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<ITokenBusiness, TokenBusiness>();
builder.Services.AddScoped<IJwt, Jwt>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<IAccountBusiness, AccountBusiness>();
builder.Services.AddScoped<ITransferBusiness, TransferBusiness>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireSignedTokens = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
});

/*builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Cache:Redis"];
    options.InstanceName = "BankAPI";
});*/


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
builder.Services.AddAuthorization();
//builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();
app.Run();