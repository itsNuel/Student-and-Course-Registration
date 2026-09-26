using myproject.Models;

namespace myproject.ViewModels
{
    public class StudentCourseViewModel
    {
        public Student Student { get; set; } = new Student();

        public Course Course { get; set; } = new Course();

        public string EnrollmentDate { get; set; } = string.Empty;
    }
}