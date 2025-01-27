using Assignment2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Controllers
{
    public class StudentController : Controller
    {
        private  CourseContext _context;

        public StudentController(CourseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEnrollment(int studentId, int courseId)
        {
            var student = await _context.Students.Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound();
            }

            var model = new EnrollmentConfirmationViewModel
            {
                StudentId = student.Id,
                StudentName = student.Name,
                CourseName = student.Course.Name,
                CourseId = courseId,
                EnrollmentStatus = student.EnrollmentStatus
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEnrollment(EnrollmentConfirmationViewModel model)
        {
            var student = await _context.Students.FindAsync(model.StudentId);

            if (student == null)
            {
                return NotFound();
            }

            // Update the student's enrollment status based on the selection (Yes or No)
            student.EnrollmentStatus = model.IsConfirmed ? EnrollmentStatus.EnrollmentConfirmed : EnrollmentStatus.EnrollmentDeclined;

            _context.Update(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("EnrollmentStatusResult", new { status = student.EnrollmentStatus });
        }
        public IActionResult EnrollmentStatusResult(EnrollmentStatus status)
        {
            // Pass the status to the view
            var model = new EnrollmentConfirmationViewModel
            {
                EnrollmentStatus = status
            };

            return View(model);
        }
    }
}

