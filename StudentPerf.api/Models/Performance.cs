using System.ComponentModel.DataAnnotations;

namespace StudentPerf.api.Models
{
    public class Performance
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int SubjectId { get; set; }
        public string Grade { get; set; } = string.Empty;
        public DateTime SubmittedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        // Navigation properties
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
    }
}
