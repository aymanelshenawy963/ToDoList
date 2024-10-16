using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Minimum length must be 3")]
        [MaxLength(50, ErrorMessage = "Minimum length must be 50")]
        public string Name { get; set; }
     
    }
}
