using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class UserData
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Minimum length must be 3")]
        [MaxLength(50, ErrorMessage = "Minimum length must be 50")]
        public string Description { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        [Required]
        public string Pdf { get; set; }
    }
}
