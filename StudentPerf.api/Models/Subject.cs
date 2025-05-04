using System.ComponentModel.DataAnnotations;

namespace StudentPerf.api.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string SubjectName { get; set; } = string.Empty;

        // Navigation properties
        public Course Course { get; set; } = null!;
        public ICollection<Performance> Performances { get; set; } = new List<Performance>();
    }
}
