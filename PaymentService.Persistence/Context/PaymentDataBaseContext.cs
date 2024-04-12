using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Contexts;
using PaymentService.Domain;

namespace PaymentService.Persistence.Context;

public class PaymentDataBaseContext : DbContext, IPaymentDataBaseContext
{

    public PaymentDataBaseContext(DbContextOptions<PaymentDataBaseContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Payment> Payments { get; set; }


}
