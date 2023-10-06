using BookManagementSystem.DBContext;
using BookManagementSystem.Interfaces;
using BookManagementSystem.Repository;
using BookManagementSystem.Repository.Interfaces;
using BookManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var launchSettingVariables = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("launchSettings.json", optional: false, reloadOnChange: true);

IConfiguration configuration = configBuilder.Build();
// Add services to the container.
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();


builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseInMemoryDatabase(configuration.GetConnectionString("DefaultConnection")));

//Addition of Serilog 
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding CORS policy
builder.Services.AddCors(p => p.AddPolicy("corsPolicy", build =>
{
    //For single domain
    //build.WithOrigins("https://localhost:7777").AllowAnyMethod().AllowAnyHeader();

    //For multiple domain
    //build.WithOrigins("https://localhost:7777", "https://localhost:8888").AllowAnyMethod().AllowAnyHeader();

    //For all domain
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
