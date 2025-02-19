using EMS.Models;
namespace EMS.Repositories;
public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> GetByIdAsync(int id);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
    Task<IEnumerable<Employee>> SearchAsync(int? departmentId, DateTime? hireDateStart, DateTime? hireDateEnd, decimal? minSalary, decimal? maxSalary, string sortBy);
}
