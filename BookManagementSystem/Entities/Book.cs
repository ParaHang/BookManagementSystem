using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Entities
{
    public class Book
    {
        [Key]
        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Id must be greater than 0.")]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Title must be within 3 to 50 Characters.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Author Name must be within 3 to 50 Characters.")]
        public string Author { get; set; } = String.Empty;
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Genre must be within 3 to 50 Characters.")]
        public string Genre { get; set; } = String.Empty;
        [Required] 
        [CurrentYearMax(ErrorMessage = "The year should not exceed the current year and should be greater than 1000.")]
        public int PublicationYear { get; set; }

    }

    //custom year check validation
    public class CurrentYearMaxAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            
            if (value is int year)
            {
                if (year <= 1000)
                    return false;
                int currentYear = DateTime.Now.Year;
                return year <= currentYear;
            }
            return false; 
        }
    }
}
