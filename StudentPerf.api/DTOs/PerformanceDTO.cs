namespace StudentPerf.api.DTOs
{
    public class PerformanceDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CourseId { get; set; }  
        public string Course { get; set; } = string.Empty;
        public int SubjectId { get; set; } 
        public string Subject { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public DateTime SubmittedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class PaginationDTO
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class PerformanceResponseDTO
    {
        public IEnumerable<PerformanceDTO> Data { get; set; } = new List<PerformanceDTO>();
        public PaginationDTO Pagination { get; set; } = new PaginationDTO();
    }
}
