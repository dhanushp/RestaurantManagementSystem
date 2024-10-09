using System.ComponentModel.DataAnnotations;
using RestaurantManagement.SharedLibrary.Models;

namespace UserService.Models
{
    public class RefreshToken : BaseEntity
    {
        [Required]
        public string Token { get; set; } // The actual refresh token string

        public DateTime ExpiresAt { get; set; } // Expiration time for the refresh token

        public bool IsRevoked { get; set; } = false; // Mark token as revoked if needed

        public DateTime? RevokedAt { get; set; } 

        public Guid UserId { get; set; } // Foreign key to associate token with user
        public User User { get; set; } // Navigation property to the user
    }
}
