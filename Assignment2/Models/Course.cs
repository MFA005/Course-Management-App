using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class Course
    {
            public int CourseId { get; set; } 
            [Required]
            public string Name { get; set; }
            [Required]
            public string Instructor { get; set; }
            [Required]
            [DataType(DataType.Date)]
            public DateTime StartDate { get; set; }
            [Required]
            [RegularExpression(@"^\d[A-Z]\d{2}$", ErrorMessage = "Room number must be in the format: 3G15")]
            public string RoomNumber { get; set; }

            
            public ICollection<Student>? Students { get; set; }

    }
}
