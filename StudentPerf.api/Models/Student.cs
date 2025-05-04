using System.ComponentModel.DataAnnotations;

namespace StudentPerf.api.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Performance> Performances { get; set; } = new List<Performance>();
    }
}
