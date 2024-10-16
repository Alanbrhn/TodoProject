using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoProject.Models;

public class ToDo
{
    public int ToDoId { get; set; }

    [Required(ErrorMessage = "Subject is required.")]
    public string Subject { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }

    public string ActivityNo { get; set; } // Unique number AC-XXXX format

    public string Status { get; set; } // Done, Canceled, Unmarked

    [Required] // Ensure UserId is set for validation
    public int UserId { get; set; } // Foreign key for the User

    [NotMapped]
   public virtual User User { get; set; } // Navigation property to the User entity

    public ToDo()
    {
        ActivityNo = GenerateActivityNo();
        Status = "Unmarked"; // Default status
    }

    public string GenerateActivityNo()
    {
        Random random = new Random();
        return $"AC-{random.Next(1000, 9999)}";
    }
}
