using EMS.Data;
using EMS.Models;
using Microsoft.EntityFrameworkCore;
namespace EMS.Repositories;
public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;
    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees.Include(e => e.Department).ToListAsync();
    }
    public async Task<Employee> GetByIdAsync(int id)
    {
        return await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id);
    }
    public async Task AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<Employee>> SearchAsync(int? departmentId, DateTime? hireDateStart, DateTime? hireDateEnd, decimal? minSalary, decimal? maxSalary, string sortBy)
    {
        var query = _context.Employees.Include(e => e.Department).AsQueryable();
        if (departmentId.HasValue)
            query = query.Where(e => e.DepartmentId == departmentId.Value);
        if (hireDateStart.HasValue)
            query = query.Where(e => e.HireDate >= hireDateStart.Value);
        if (hireDateEnd.HasValue)
            query = query.Where(e => e.HireDate <= hireDateEnd.Value);
        if (minSalary.HasValue)
            query = query.Where(e => e.Salary >= minSalary.Value);
        if (maxSalary.HasValue)
            query = query.Where(e => e.Salary <= maxSalary.Value);
        query = sortBy switch
        {
            "Name" => query.OrderBy(e => e.FullName),
            "HireDate" => query.OrderBy(e => e.HireDate),
            "Salary" => query.OrderBy(e => e.Salary),
            _ => query.OrderBy(e => e.Id)
        };
        return await query.ToListAsync();
    }
}
