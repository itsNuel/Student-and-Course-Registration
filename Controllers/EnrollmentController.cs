using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myproject.Models;
using myproject.Data;

namespace myproject.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var enrollments = _context.Enrollments
                .Include(enrollment => enrollment.Student)
                .Include(enrollment => enrollment.Course)
                .OrderBy(enrollment => enrollment.Id)
                .ToList();

            return View(enrollments);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Enrollment enrollment)
        {
            var studentExists = _context.Students
                .Any(student => student.Id == enrollment.StudentId);

            var courseExists = _context.Courses
                .Any(course => course.Id == enrollment.CourseId);

            if (!studentExists)
            {
                ModelState.AddModelError(
                    nameof(enrollment.StudentId),
                    "Please select a valid student.");
            }

            if (!courseExists)
            {
                ModelState.AddModelError(
                    nameof(enrollment.CourseId),
                    "Please select a valid course.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(enrollment.StudentId, enrollment.CourseId);
                return View(enrollment);
            }

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = _context.Enrollments.Find(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            LoadDropdowns(enrollment.StudentId, enrollment.CourseId);
            return View(enrollment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public IActionResult Edit(int id, Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            var studentExists = _context.Students
                .Any(student => student.Id == enrollment.StudentId);

            var courseExists = _context.Courses
                .Any(course => course.Id == enrollment.CourseId);

            if (!studentExists)
            {
                ModelState.AddModelError(
                    nameof(enrollment.StudentId),
                    "Please select a valid student.");
            }

            if (!courseExists)
            {
                ModelState.AddModelError(
                    nameof(enrollment.CourseId),
                    "Please select a valid course.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(enrollment.StudentId, enrollment.CourseId);
                return View(enrollment);
            }

            var savedEnrollment = _context.Enrollments.Find(id);

            if (savedEnrollment == null)
            {
                return NotFound();
            }

            savedEnrollment.StudentId = enrollment.StudentId;
            savedEnrollment.CourseId = enrollment.CourseId;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = _context.Enrollments
                .Include(enrollment => enrollment.Student)
                .Include(enrollment => enrollment.Course)
                .FirstOrDefault(enrollment => enrollment.Id == id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var enrollment = _context.Enrollments.Find(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(int? studentId = null, int? courseId = null)
        {
            ViewBag.StudentId = new SelectList(
                _context.Students.OrderBy(student => student.Name).ToList(),
                "Id",
                "Name",
                studentId);

            ViewBag.CourseId = new SelectList(
                _context.Courses.OrderBy(course => course.CourseName).ToList(),
                "Id",
                "CourseName",
                courseId);
        }
    }
}