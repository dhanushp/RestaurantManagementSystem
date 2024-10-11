using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Interfaces;
using PaymentService.Models;
using RestaurantManagement.SharedDataLibrary.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentService.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> GetPaymentByTransactionIdAsync(string transactionId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdatePaymentStatusAsync(string transactionId, PaymentStatus status)
        {
            var payment = await GetPaymentByTransactionIdAsync(transactionId);
            if (payment != null)
            {
                payment.Status = status;
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }
            return payment;
        }

        public async Task<Payment> GetPaymentByOrderIdAsync(string orderId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            // Mark the entity as modified
            _context.Entry(payment).State = EntityState.Modified;
            payment.UpdatedAt = DateTime.Now;

            // Save the changes to the database
            _context.SaveChangesAsync();
        }
    }
}
