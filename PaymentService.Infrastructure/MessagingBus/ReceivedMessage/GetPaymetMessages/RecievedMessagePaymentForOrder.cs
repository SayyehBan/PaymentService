using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentService.Application.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SayyehBanTools.MessagingBus.RabbitMQ.Connection;
using SayyehBanTools.MessagingBus.RabbitMQ.Model;
using System.Text;

namespace PaymentService.Infrastructure.MessagingBus.ReceivedMessage.GetPaymetMessages;

public class RecievedMessagePaymentForOrder : BackgroundService
{
    private readonly RabbitMqConnectionSettings _rabbitMqConnectionSettings;
    private readonly RabbitMQConnection rabbitMQConnection;
    private readonly string _queueName;
    private readonly IPaymentService paymentService;
    public RecievedMessagePaymentForOrder(IPaymentService paymentService,RabbitMQConnection rabbitMQConnection, IOptions<RabbitMqConnectionSettings> rabbitMqConnectionSettings)
    {
        this.paymentService = paymentService;
        _rabbitMqConnectionSettings = rabbitMqConnectionSettings.Value;
        _queueName = _rabbitMqConnectionSettings.queue;
        this.rabbitMQConnection = rabbitMQConnection;
        this.rabbitMQConnection.CreateRabbitMQConnection();
        this.rabbitMQConnection.Channel = rabbitMQConnection.Connection.CreateModel();
        this.rabbitMQConnection.Channel.QueueDeclare(queue: _queueName, durable: true,
            exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(rabbitMQConnection.Channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonConvert
            .DeserializeObject<MessagePaymentDto>(content);
            var resultHandeleMessage =
            HandleMessage(message.OrderId, message.Amount);

            if (resultHandeleMessage)
                rabbitMQConnection.Channel.BasicAck(ea.DeliveryTag, false);
        };
        rabbitMQConnection.Channel.BasicConsume(queue: _queueName, false, consumer);
        return Task.CompletedTask;
    }
    private bool HandleMessage(Guid OrderId, int Amount)
    {
        return paymentService.CreatePayment(OrderId, Amount);
    }
}
public class MessagePaymentDto
{
    public Guid OrderId { get; set; }
    public int Amount { get; set; }
}