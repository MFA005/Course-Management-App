namespace Assignment2.Models
{
    public class EnrollmentConfirmationViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public bool IsConfirmed { get; set; }
        public EnrollmentStatus EnrollmentStatus { get; set; }
    }
}
