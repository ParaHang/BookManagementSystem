using BookManagementSystemOrder.DBContext;
using BookManagementSystemOrder.Interfaces;
using BookManagementSystemOrder.Repository;
using BookManagementSystemOrder.Repository.Interfaces;
using BookManagementSystemOrder.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration configuration = configBuilder.Build();
// Add services to the container.
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<IOrderService,OrderService>();


builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseInMemoryDatabase(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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