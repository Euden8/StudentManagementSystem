using System.ComponentModel.DataAnnotations;
using StudentManagementSystem.Models;

namespace StudentApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Range(1, 5)]
        public int Credits { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}