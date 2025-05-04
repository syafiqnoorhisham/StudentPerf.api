using System.ComponentModel.DataAnnotations;

namespace StudentPerf.api.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public ICollection<Performance> Performances { get; set; } = new List<Performance>();
    }
}
