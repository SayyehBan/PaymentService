using SayyehBanTools.MessagingBus.RabbitMQ.Model;

namespace PaymentService.Infrastructure.MessagingBus.Messages;

public class PaymentIsDoneMessage : BaseMessage
{
    public Guid OrderId { get; set; }

}
