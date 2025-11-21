using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

// Thêm namespace để đồng bộ với AppIdentityDbContext
namespace SportsStore.Areas.Identity.Data 
{
    public class ApplicationUser : IdentityUser
    {
        // Thuộc tính tùy chỉnh cần thêm vào
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;
    }
}