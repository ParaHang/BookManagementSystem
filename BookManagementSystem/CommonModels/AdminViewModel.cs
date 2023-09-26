using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Common
{
    public class AdminViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
    }
}
