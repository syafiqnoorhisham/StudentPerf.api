using Microsoft.EntityFrameworkCore;
using StudentPerf.api.Models;

namespace StudentPerf.api.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StudentPerfContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                await context.Database.MigrateAsync();
                await SeedDataAsync(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task SeedDataAsync(StudentPerfContext context)
        {
            // Only seed if the database is empty
            if (await context.Students.AnyAsync())
            {
                return;
            }

            // Add students
            var students = new List<Student>
            {
                new Student { Name = "John Doe", Email = "john.doe@example.com" },
                new Student { Name = "Jane Smith", Email = "jane.smith@example.com" },
                new Student { Name = "Bob Johnson", Email = "bob.johnson@example.com" },
                new Student { Name = "Alice Williams", Email = "alice.williams@example.com" },
                new Student { Name = "Charlie Brown", Email = "charlie.brown@example.com" }
            };

            await context.Students.AddRangeAsync(students);
            await context.SaveChangesAsync();

            // Add courses
            var courses = new List<Course>
            {
                new Course { CourseCode = "CS101", CourseName = "Computer Science" },
                new Course { CourseCode = "MTH201", CourseName = "Mathematics" },
                new Course { CourseCode = "ENG301", CourseName = "English Literature" },
                new Course { CourseCode = "PHY401", CourseName = "Physics" }
            };

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();

            // Add subjects
            var subjects = new List<Subject>
            {
                new Subject { CourseId = courses[0].Id, SubjectName = "Database Management" },
                new Subject { CourseId = courses[0].Id, SubjectName = "Programming Fundamentals" },
                new Subject { CourseId = courses[1].Id, SubjectName = "Calculus" },
                new Subject { CourseId = courses[1].Id, SubjectName = "Statistics" },
                new Subject { CourseId = courses[2].Id, SubjectName = "English Composition" },
                new Subject { CourseId = courses[2].Id, SubjectName = "Literary Analysis" },
                new Subject { CourseId = courses[3].Id, SubjectName = "Mechanics" },
                new Subject { CourseId = courses[3].Id, SubjectName = "Electromagnetism" }
            };

            await context.Subjects.AddRangeAsync(subjects);
            await context.SaveChangesAsync();

            // Add performance records
            var random = new Random();
            var grades = new[] { "A", "B", "C", "D", "F" };
            var performances = new List<Performance>();

            foreach (var student in students)
            {
                foreach (var course in courses)
                {
                    var courseSubjects = subjects.Where(s => s.CourseId == course.Id).ToList();
                    foreach (var subject in courseSubjects)
                    {
                        var submittedDate = DateTime.Now.AddDays(-random.Next(1, 30));
                        var modifiedDate = submittedDate.AddHours(random.Next(1, 48));
                        
                        performances.Add(new Performance
                        {
                            StudentId = student.Id,
                            CourseId = course.Id,
                            SubjectId = subject.Id,
                            Grade = grades[random.Next(grades.Length)],
                            SubmittedOn = submittedDate,
                            ModifiedOn = modifiedDate
                        });
                    }
                }
            }

            await context.Performances.AddRangeAsync(performances);
            await context.SaveChangesAsync();
        }
    }
}
