using Microsoft.EntityFrameworkCore;

namespace Assignment2.Models
{
    public class CourseContext : DbContext
    {
        public CourseContext(DbContextOptions<CourseContext> options)
        : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Initial data for Course
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseId = 1,
                    Name = "Introduction to Programming",
                    Instructor = "Dr. Alice Johnson",
                    StartDate = new DateTime(2024, 09, 01),
                    RoomNumber = "3G15"
                },
                new Course
                {
                    CourseId = 2,
                    Name = "Web Development",
                    Instructor = "Prof. Bob Smith",
                    StartDate = new DateTime(2024, 09, 01),
                    RoomNumber = "2A22"
                }
            );

            // Seed initial data for Student
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    EnrollmentStatus = EnrollmentStatus.ConfirmationMessageNotSent,
                    CourseId = 1 // Linked to "Introduction to Programming"
                },
                new Student
                {
                    Id = 2,
                    Name = "Jane Doe",
                    Email = "janedoe@example.com",
                    EnrollmentStatus = EnrollmentStatus.ConfirmationMessageNotSent,
                    CourseId = 2 // Linked to "Web Development"
                },
                new Student
                {
                    Id = 3,
                    Name = "Mark Smith",
                    Email = "marksmith@example.com",
                    EnrollmentStatus = EnrollmentStatus.ConfirmationMessageNotSent,
                    CourseId = 1 // Linked to "Introduction to Programming"
                });
        }
    }
}
