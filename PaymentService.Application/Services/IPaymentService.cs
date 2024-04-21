using PaymentService.Application.Contexts;
using PaymentService.Domain;

namespace PaymentService.Application.Services;

public interface IPaymentService
{
    PaymentDto GetPaymentofOrder(Guid OrderId);
    PaymentDto GetPayment(Guid PaymentId);
    bool CreatePayment(Guid OrderId, int Amount);
    void PayDone(Guid Paymentid, string Authority, long RefId);
}
public class RPaymentServiceConcrete : IPaymentService
{
    private readonly IPaymentDataBaseContext context;

    public RPaymentServiceConcrete(IPaymentDataBaseContext context)
    {
        this.context = context;
    }

    public bool CreatePayment(Guid OrderId, int Amount)
    {
        var order = GetOrder(OrderId, Amount);
        var payment = context.Payments.SingleOrDefault(p => p.OrderId == order.Id);
        if (payment != null)
        {
            return true;
        }
        else
        {
            Payment newPayment = new Payment()
            {
                Amount = Amount,
                Id = Guid.NewGuid(),
                IsPay = false,
                Order = order,
                Authority=""
            };
            context.Payments.Add(newPayment);
            context.SaveChanges();
            return true;
        }
    }

    private Order GetOrder(Guid OrderId, int Amount)
    {
        var order = context.Orders.SingleOrDefault(p => p.Id == OrderId);
        if (order != null)
        {
            if (order.Amount != Amount)
            {
                order.Amount = Amount;
                context.SaveChanges();
            }
            return order;
        }
        else
        {
            Order newOrder = new Order()
            {
                Amount = Amount,
                Id = OrderId,
            };
            context.Orders.Add(newOrder);
            context.SaveChanges();
            return newOrder;
        }

    }

    public PaymentDto GetPayment(Guid PaymentId)
    {
        var payment = context.Payments.SingleOrDefault(p => p.Id == PaymentId);
        if (payment != null)
        {
            return new PaymentDto
            {
                Amount = payment.Amount,
                IsPay = payment.IsPay,
                OrderId = payment.OrderId,
                PaymentId = payment.Id
            };
        }
        else
            return null;
    }

    public PaymentDto GetPaymentofOrder(Guid OrderId)
    {
        var payment = context.Payments.SingleOrDefault(p => p.OrderId == OrderId);
        if (payment != null)
        {
            return new PaymentDto
            {
                Amount = payment.Amount,
                IsPay = payment.IsPay,
                OrderId = payment.OrderId,
                PaymentId = payment.Id
            };
        }
        else
            return null;
    }

    public void PayDone(Guid Paymentid, string Authority, long RefId)
    {
        var payment = context.Payments.SingleOrDefault(p => p.Id == Paymentid);
        payment.DatePay = DateTime.UtcNow;
        payment.IsPay = true;
        payment.Authority = Authority;
        payment.RefId = RefId;
        context.SaveChanges();
    }
}
