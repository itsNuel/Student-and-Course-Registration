// namespace myproject.Models
// {
//     public class Student
//     {
//         public int Id { get; set; }

//         public string Name { get; set; } = string.Empty;

//         public string Program { get; set; } = string.Empty;

//         public string Email { get; set; } = string.Empty;

//         public string Phone { get; set; } = string.Empty;

//         public int? CourseId { get; set; }

//         public Course? Course { get; set; }

//         public DateTime? EnrollmentDate { get; set; }
//     }
// }


using System.ComponentModel.DataAnnotations;

namespace myproject.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the student's name.")]
        [StringLength(100,
            ErrorMessage = "Name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s'-]+$",
            ErrorMessage = "Name must contain only letters, spaces, hyphens, or apostrophes.")]
        [Display(Name = "Student Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the student's program.")]
        [StringLength(100,
            ErrorMessage = "Program cannot exceed 100 characters.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])[a-zA-Z0-9\s&()'-]+$",
            ErrorMessage = "Program must contain at least one letter and only valid characters.")]
        [Display(Name = "Program Name")]
        public string Program { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(254, ErrorMessage = "Email address cannot exceed 254 characters.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Enter a valid phone number.")]
        [StringLength(30, ErrorMessage = "Phone number cannot exceed 30 characters.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;
    }
}