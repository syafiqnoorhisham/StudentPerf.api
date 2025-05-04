using Microsoft.EntityFrameworkCore;
using StudentPerf.api.Data;
using StudentPerf.api.DTOs;
using StudentPerf.api.Models;
using System.Linq.Dynamic.Core;

namespace StudentPerf.api.Repositories
{
    public interface IPerformanceRepository
    {
        Task<(List<PerformanceDTO> Data, int TotalItems, int TotalPages)> GetPerformancesAsync(
            string search = null,
            int? course = null,
            int? subject = null,
            int page = 1,
            int pageSize = 10,
            string sortBy = "StudentName",
            string sortDirection = "asc");
    }

    public class PerformanceRepository : IPerformanceRepository
    {
        private readonly StudentPerfContext _context;

        public PerformanceRepository(StudentPerfContext context)
        {
            _context = context;
        }

        public async Task<(List<PerformanceDTO> Data, int TotalItems, int TotalPages)> GetPerformancesAsync(
            string search = null,
            int? course = null,
            int? subject = null,
            int page = 1,
            int pageSize = 10,
            string sortBy = "StudentName",
            string sortDirection = "asc")
        {
            var query = _context.Performances
                .Include(p => p.Student)
                .Include(p => p.Course)
                .Include(p => p.Subject)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(p =>
                    p.Student.Name.ToLower().Contains(search) ||
                    p.Course.CourseName.ToLower().Contains(search) ||
                    p.Subject.SubjectName.ToLower().Contains(search));
            }

            if (course.HasValue)
            {
                // Filter by course ID
                var courses = await _context.Courses.ToListAsync();
                if (course.Value >= 1 && course.Value <= courses.Count)
                {
                    var courseId = courses[course.Value - 1].Id;
                    query = query.Where(p => p.CourseId == courseId);
                }
            }

            if (subject.HasValue)
            {
                // Filter by subject ID
                var subjects = await _context.Subjects.ToListAsync();
                if (subject.Value >= 1 && subject.Value <= subjects.Count)
                {
                    var subjectId = subjects[subject.Value - 1].Id;
                    query = query.Where(p => p.SubjectId == subjectId);
                }
            }

            // Get total count for pagination
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Apply paging
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            // Create a DTO projection query
            var dtoQuery = query.Select(p => new PerformanceDTO
            {
                Id = p.Id,
                StudentId = p.StudentId,
                StudentName = p.Student.Name,
                CourseId = p.Course.Id,
                Course = p.Course.CourseName,
                SubjectId = p.Subject.Id,
                Subject = p.Subject.SubjectName,
                Grade = p.Grade,
                SubmittedOn = p.SubmittedOn,
                ModifiedOn = p.ModifiedOn
            });

            // Apply sorting
            switch (sortBy.ToLower())
            {
                case "studentid":
                    dtoQuery = sortDirection.ToLower() == "desc"
                        ? dtoQuery.OrderByDescending(p => p.StudentId)
                        : dtoQuery.OrderBy(p => p.StudentId);
                    break;
                case "studentname":
                    dtoQuery = sortDirection.ToLower() == "desc"
                        ? dtoQuery.OrderByDescending(p => p.StudentName)
                        : dtoQuery.OrderBy(p => p.StudentName);
                    break;
                case "course":
                    dtoQuery = sortDirection.ToLower() == "desc"
                        ? dtoQuery.OrderByDescending(p => p.Course)
                        : dtoQuery.OrderBy(p => p.Course);
                    break;
                case "subject":
                    dtoQuery = sortDirection.ToLower() == "desc"
                        ? dtoQuery.OrderByDescending(p => p.Subject)
                        : dtoQuery.OrderBy(p => p.Subject);
                    break;
                case "grade":
                    dtoQuery = sortDirection.ToLower() == "desc"
                        ? dtoQuery.OrderByDescending(p => p.Grade)
                        : dtoQuery.OrderBy(p => p.Grade);
                    break;
                case "submittedon":
                    dtoQuery = sortDirection.ToLower() == "desc"
                        ? dtoQuery.OrderByDescending(p => p.SubmittedOn)
                        : dtoQuery.OrderBy(p => p.SubmittedOn);
                    break;
                case "modifiedon":
                    dtoQuery = sortDirection.ToLower() == "desc"
                        ? dtoQuery.OrderByDescending(p => p.ModifiedOn)
                        : dtoQuery.OrderBy(p => p.ModifiedOn);
                    break;
                default:
                    dtoQuery = dtoQuery.OrderBy(p => p.StudentName);
                    break;
            }

            // Get paged data
            var pagedData = await dtoQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pagedData, totalItems, totalPages);
        }
    }
}