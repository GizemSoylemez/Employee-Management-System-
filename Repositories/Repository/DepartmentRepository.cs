using EMS.Data;
using EMS.Models;
using Microsoft.EntityFrameworkCore;
namespace EMS.Repositories;
public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;
    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments.ToListAsync();
    }
    public async Task<Department> GetByIdAsync(int id)
    {
        return await _context.Departments.FindAsync(id);
    }
    public async Task AddAsync(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Department department)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var dept = await _context.Departments.FindAsync(id);
        if (dept != null)
        {
            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
        }
    }
}
