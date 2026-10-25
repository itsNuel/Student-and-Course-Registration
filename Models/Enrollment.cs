using System.ComponentModel.DataAnnotations;

namespace myproject.Models
{
    public class Enrollment
    {
        public int Id {get; set; }

        [Required]
        public int StudentId {get; set; }

        [Required]
        public int CourseId {get; set; }

        public Student? Student {get; set; }

        public Course? Course {get; set;}
    }

}