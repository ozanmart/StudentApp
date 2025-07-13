using System.ComponentModel.DataAnnotations;

namespace StudentApp.Models;

public class Student
{
    public int Id { get; init; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(30, ErrorMessage = "Name cannot exceed 40 characters")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = null!;
    
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; init; }
    
    [Required(ErrorMessage = "Major is required")]
    [MaxLength(50, ErrorMessage = "Major cannot exceed 50 characters")]
    public string Major { get; set; } = null!;
}