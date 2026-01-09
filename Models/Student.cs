using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models;

public class Student
{
    public int Id { get; set; }
        
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }
        
    [Required]
    [StringLength(50)]
    public string LastName { get; set; }
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public ICollection<Enrollment>? Enrollments { get; set; }
    
}