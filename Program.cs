using System.Text;
using Server.Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Infrastructure;
using Server.Infrastructure.Repositories.ClientConnection;
using Server.Infrastructure.Repositories.Group;
using Server.Infrastructure.Repositories.Message;
using Server.Infrastructure.Repositories.User;
using Server.Domain.Interface.ClientConnection;
using Server.Domain.Interface.Group;
using Server.Domain.Interface.Message;
using Server.Domain.Interface.User;
using Hangfire;
using Hangfire.Storage.SQLite;
using Server.Filters;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "ChatAppOrigins";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ChatDbContext>(options => options.UseSqlite(connectionString));

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer",
        Description = "Please insert JWT token into field"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services
    .AddAuthentication()
    .AddJwtBearer("JwtScheme", options => {
        options.TokenValidationParameters = new TokenValidationParameters 
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,                
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        };
});

//configure cors
builder.Services.AddCors(options =>
     {
         options.AddPolicy(CorsPolicy,
             _builder =>
             {
                 _builder
                 .WithOrigins(builder.Configuration["Origins:Local"], builder.Configuration["Origins:Production"]) 
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials();
             });
     });

builder.Services.AddControllers(options => {
    options.Filters.Add(new AuthorizeFilter
        (new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes("JwtScheme")
            .Build()));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClientConnectionRepository, ClientConnectionRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();


//add hangfire
builder.Services.AddHangfire(x =>
{
    x.UseSQLiteStorage(connectionString);
});

var app = builder.Build();

// Configure the swagger UI

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    dbContext.Database.Migrate();  // Apply any pending migrations
}

app.UseHangfireServer();

app.UseHangfireDashboard("/hangfire-dashboard", new DashboardOptions()
{
    Authorization = new[] { new HangFireAuthorizationFilter() },
    DashboardTitle = "GamaLearn ChatApp Jobs",
    AppPath = "/hangfire-dashboard"
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");

app.UseCors(CorsPolicy);

app.Run();
