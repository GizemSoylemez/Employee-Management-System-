using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EMS.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet("department-statistics")]
    public async Task<ActionResult<IEnumerable<DepartmentStatistic>>> GetDepartmentStatistics()
    {
        var stats = await _context.DepartmentStatistics
            .FromSqlRaw("EXEC sp_GetDepartmentStatistics")
            .ToListAsync();
        return Ok(stats);
    }
}
