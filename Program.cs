using FirebaseAdmin;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Interface;
using GoChauffeurWebApi.Service;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string myAllowSpecificOrigins = "myAllowSpecificOrigins";
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddControllers();
builder.Services.AddHttpClient<IFCMService, FCMService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoChauffeur", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                        new string[] { }
                    }
                });
});
builder.Services.AddDbContext<DataContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
string logFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
string logFilePath = Path.Combine(logFolderPath, "apilog-.log");
Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
                    };
                });
builder.Services.AddCors(Options =>
{
    Options.AddPolicy(name: myAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", "http://localhost:55157", "https://gochauffeurs.in", "https://gochauffeurs.codefactstech.com", "https://gochauffeursadmin.codefactstech.com", "https://admin.gochauffeurs.in")
          .AllowAnyMethod()
            .AllowAnyHeader();
        });

});
var app = builder.Build();

// Configure the HTTP request pipeline.

//var uploadsPath = Path.Combine(builder.Environment.WebRootPath, "Uploads");
var uploadsPath = builder.Configuration["UploadsPath"]
    ?? Path.Combine(builder.Environment.WebRootPath, "Uploads");

if (!Directory.Exists(uploadsPath))
{
	Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(uploadsPath),
	RequestPath = "/Uploads",
	OnPrepareResponse = ctx =>
	{
		var allowedOrigins = new[]
		{
			"http://localhost:4200",
			"https://gochauffeurs.in",
			"https://gochauffeursadmin.codefactstech.com",
			"https://admin.gochauffeurs.in"
		};

		var origin = ctx.Context.Request.Headers["Origin"].ToString();

		if (allowedOrigins.Contains(origin))
		{
			ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
			ctx.Context.Response.Headers.Append("Access-Control-Allow-Methods", "GET");
			ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");
		}
	}
});

app.MapGet("/debug-path", (IWebHostEnvironment env) => new {
	ContentRoot = env.ContentRootPath,
	UploadsPath = Path.Combine(env.ContentRootPath, "uploads"),
	Exists = Directory.Exists(Path.Combine(env.ContentRootPath, "uploads"))
});
app.UseSwagger();
app.Use(async (context, next) =>
{
	if (context.Request.Path == "/" || context.Request.Path == "/index.html")
	{
		var basePath = context.Request.PathBase.Value ?? "";
		context.Response.Redirect($"{basePath}/swagger/index.html");
		return;
	}
	await next();
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoChauffeur API v1");
});

app.UseCors("myAllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
