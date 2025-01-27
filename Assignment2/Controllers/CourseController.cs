using Assignment2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Mail;
using System.Net;


namespace Assignment2.Controllers
{
    public class CourseController : Controller
    {
        private CourseContext _context;

        public CourseController(CourseContext context)
        {
            _context = context;
        }
        // get index action method
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }
        public async Task<IActionResult> Manage(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Students)  // includes students for this course
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // method:POST: Course/AddStudent/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(int id, string studentName, string studentEmail)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            var student = new Student
            {
                Name = studentName,
                Email = studentEmail,
                EnrollmentStatus = EnrollmentStatus.ConfirmationMessageNotSent,
                CourseId = id
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Manage), new { id = id });  // Redirect back to the manage page
        }
        //post action method for sending confirmation messages
        [HttpPost]
        public async Task<IActionResult> SendConfirmationMessages(int id)
        {
            var course = _context.Courses
                .Include(c => c.Students)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            // Email logic
            string fromAddress = "yourmail@gmail.com";// Please enter test email address
            string generatedPassword = "yourgeneratedpassword";// Please enter generated app password
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromAddress, generatedPassword),
                EnableSsl = true,
            };

            foreach (var student in course.Students)
            {

                if (student.EnrollmentStatus != EnrollmentStatus.ConfirmationMessageNotSent)
                {
                    // Skip sending the email if the enrollment is already confirmed
                    continue;
                }

                var toAddress = student.Email;

                string? confirmationLink = Url.Action(
                    "ConfirmEnrollment",
                    "Student",
                    new { studentId = student.Id, courseId = course.CourseId },
                    protocol: HttpContext.Request.Scheme);

                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(fromAddress),
                    Subject = "Course Enrollment Confirmation",
                    Body = $@"
                        <h1>Enrollment Confirmation</h1>
                        <p>Hello {student.Name},</p>
                        <p>You are requested to confirm your enrollment in the course {course.Name} starting on {course.StartDate:MM/dd/yyyy}.</p>
                        <p>Please click the link below to confirm your enrollment:</p>
                        <a href='{confirmationLink}'>Confirm Enrollment</a>
                        <p>If you do not wish to enroll, please ignore this message.</p>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toAddress);

                try
                {
                   await smtpClient.SendMailAsync(mailMessage);

                    student.EnrollmentStatus = EnrollmentStatus.ConfirmationMessageSent;
                    _context.Students.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Handle the exception (logging or displaying an error message)
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }

            // Redirect to a confirmation page or back to course management page
            return RedirectToAction("Manage", new { id = course.CourseId });
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Manage", new { id = course.CourseId });
            }
            return View(course);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing course from the database
                var existingCourse = await _context.Courses.FindAsync(id);

                if (existingCourse == null)
                {
                    return NotFound();  
                }

                // Update the properties that have changed
                existingCourse.Name = course.Name;          
                existingCourse.Instructor = course.Instructor; 
                existingCourse.StartDate = course.StartDate;   
                existingCourse.RoomNumber = course.RoomNumber;  

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction("Manage", new { id = id }); // Redirect to the manage course page
            }

            
            return View(course);
        }

       
    }
}
