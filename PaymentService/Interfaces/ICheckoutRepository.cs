using RestaurantManagement.SharedDataLibrary.DTOs.Payment;

namespace PaymentService.Interfaces
{
    public interface ICheckoutRepository
    {
        Task<string> GetPayPalAccessToken();
        Task<PayPalOrderResponseDTO> CreateOrder(PayPalCreateOrderDTO createOrderDTO);
        Task<PayPalCaptureOrderResponseDTO> CaptureOrder(string orderId);
    }
}
