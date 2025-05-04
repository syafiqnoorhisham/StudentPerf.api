using Microsoft.EntityFrameworkCore;
using StudentPerf.api.Data;
using StudentPerf.api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS policy for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<StudentPerfContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();

var app = builder.Build();

// Enable CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();



app.MapControllers();

app.Run();
