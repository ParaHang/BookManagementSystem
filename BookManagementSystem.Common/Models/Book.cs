using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Common.Models
{
    public class Book
    {
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty; 
        public string Author { get; set; } = String.Empty; 
        public string Genre { get; set; } = String.Empty; 
        public int PublicationYear { get; set; }
    }
}
