using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{

    public enum EnrollmentStatus
    {
        ConfirmationMessageNotSent,
        ConfirmationMessageSent,
        EnrollmentConfirmed,
        EnrollmentDeclined
    }

    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;
        [Required]
        public EnrollmentStatus EnrollmentStatus { get; set; } = EnrollmentStatus.ConfirmationMessageNotSent;

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = null!;
    }
}
