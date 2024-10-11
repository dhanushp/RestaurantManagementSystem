using PaymentService.Models;
using RestaurantManagement.SharedDataLibrary.Enums;

namespace PaymentService.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentByTransactionIdAsync(string transactionId);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentStatusAsync(string transactionId, PaymentStatus status);
        Task<Payment> GetPaymentByOrderIdAsync(string orderId);

        Task UpdatePaymentAsync(Payment payment);
    }

}
