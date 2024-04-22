using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Contexts;
using PaymentService.Application.Services;
using PaymentService.Infrastructure.MessagingBus.ReceivedMessage.GetPaymetMessages;
using PaymentService.Persistence.Context;
using SayyehBanTools.ConfigureService;
using SayyehBanTools.ConnectionDB;
using SayyehBanTools.MessagingBus.RabbitMQ.Model;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IPaymentDataBaseContext, PaymentDataBaseContext>();
builder.Services.AddDbContext<PaymentDataBaseContext>(p =>
p.UseSqlServer(SqlServerConnection.ConnectionString("urMcjpPHmP8SS3v70kndBw==", "haUa5gTQBP9rckJh9Za4/A==", "QbLwKXetr0dBjiUhNUNpvQ==", "70D4ul+uXR+y357iQaTIJw==", "5m6h7m9i9386b9c6", "axoz52ew9510gnmc")),ServiceLifetime.Singleton); 
var configureServices = new ConfigureServicesRabbitMQ();
configureServices.ConfigureService(builder.Services);
//RabbitMQ
builder.Services.Configure<RabbitMqConnectionSettings>(builder.Configuration
    .GetSection("RabbitMq"));
builder.Services.AddTransient<IPaymentService, RPaymentServiceConcrete>();
builder.Services.AddHostedService<RecievedMessagePaymentForOrder>();

builder.Services.AddTransient<IPaymentService, RPaymentServiceConcrete>();

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

app.Run();
