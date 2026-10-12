using Microsoft.AspNetCore.Mvc;
using myproject.Data;
using myproject.Models;
using myproject.ViewModels;

namespace myproject.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var students = _context.Students
                .OrderBy(student => student.Id)
                .ToList();

            return View(students);
        }

        public IActionResult Create()
        {
            return View(new StudentCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    Name = model.Name,
                    Program = model.Program
                };

                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}