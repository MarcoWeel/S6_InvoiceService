using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using InvoiceService.Data;
using InvoiceService.Controllers;
using InvoiceService.Services.Interfaces;
using InvoiceService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets/appsettings.secrets.json", true);

var conStrBuilder = new MySqlConnectionStringBuilder(
    builder.Configuration.GetConnectionString("DBConnectionString"));
var connection = conStrBuilder.ConnectionString;

builder.Services.AddDbContext<InvoiceServiceContext>();
//options =>
//    options.UseMySql(connection, ServerVersion.AutoDetect(connection)));


// Add services to the container.
builder.Services.AddSingleton<IMessagingService, MessagingService>();

//builder.Services.AddSingleton<IProductService, ProductService>();
//builder.Services.AddSingleton<IMaterialService, MaterialService>();
builder.Services.AddSingleton<IInvoiceService, InvoiceService.Services.InvoicesService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.Services.GetRequiredService<IProductService>().SubscribeToGlobal();
//app.Services.GetRequiredService<IMaterialService>().SubscribeToGlobal();
app.Services.GetRequiredService<IInvoiceService>().SubscribeToGlobal();


app.Run();
