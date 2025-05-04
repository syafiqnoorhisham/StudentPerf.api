using Microsoft.AspNetCore.Mvc;
using StudentPerf.api.DTOs;
using StudentPerf.api.Repositories;

namespace StudentPerf.api.Controllers
{
    [ApiController]
    [Route("api/performance")]
    public class PerformanceController : ControllerBase
    {
        private readonly ILogger<PerformanceController> _logger;
        private readonly IPerformanceRepository _performanceRepository;

        public PerformanceController(
            ILogger<PerformanceController> logger,
            IPerformanceRepository performanceRepository)
        {
            _logger = logger;
            _performanceRepository = performanceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PerformanceResponseDTO>> GetPerformances(
            string? search = null,
            int? courseId = null,
            int? subjectId = null,
            int page = 1,
            int pageSize = 10,
            string sortBy = "StudentName",
            string sortDirection = "asc")
        {
            try
            {
                var (pagedData, totalItems, totalPages) = await _performanceRepository.GetPerformancesAsync(
                    search, courseId, subjectId, page, pageSize, sortBy, sortDirection);

                // Prepare response
                var response = new PerformanceResponseDTO
                {
                    Data = pagedData,
                    Pagination = new PaginationDTO
                    {
                        TotalItems = totalItems,
                        TotalPages = totalPages,
                        CurrentPage = page,
                        PageSize = pageSize
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving performance data");
                return StatusCode(500, "An error occurred while retrieving data");
            }
        }
    }
}