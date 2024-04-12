using Microsoft.EntityFrameworkCore;
using PaymentService.Domain;

namespace PaymentService.Application.Contexts;

public interface IPaymentDataBaseContext
{
    DbSet<Order> Orders { get; set; }
    DbSet<Payment> Payments { get; set; }
    int SaveChanges();
}