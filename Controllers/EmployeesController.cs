using EMS.Models;
using EMS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace EMS.Controllers;

// Çalışan işlemlerini yönettiğimiz controller
public class EmployeesController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeesController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    // Çalışan listesi ve filtreleme
    public async Task<IActionResult> Index(int? departmentId, DateTime? hireDateStart, 
        DateTime? hireDateEnd, decimal? minSalary, decimal? maxSalary, string sortBy)
    {
        var departments = await _departmentRepository.GetAllAsync();
        ViewBag.Departments = departments.Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.Name
        });

        var employees = await _employeeRepository.SearchAsync(departmentId, hireDateStart, 
            hireDateEnd, minSalary, maxSalary, sortBy);
        return View(employees);
    }

    public async Task<IActionResult> Details(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            return NotFound();
        return View(employee);
    }

    public async Task<IActionResult> Create(int? departmentId = null)
    {
        var departments = await _departmentRepository.GetAllAsync();
        ViewBag.Departments = departments.Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.Name,
            Selected = departmentId.HasValue && d.Id == departmentId.Value
        });
        
        var employee = new Employee();
        if (departmentId.HasValue)
        {
            employee.DepartmentId = departmentId.Value;
        }
        
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        try 
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                
                TempData["Error"] = string.Join(", ", errors);
            }
            
            if (ModelState.IsValid)
            {
                await _employeeRepository.AddAsync(employee);
                TempData["Success"] = "Çalışan başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Departments = (await _departmentRepository.GetAllAsync())
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name,
                    Selected = d.Id == employee.DepartmentId
                });
                
            return View(employee);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Çalışan eklenirken bir hata oluştu: " + ex.Message);
            
            ViewBag.Departments = (await _departmentRepository.GetAllAsync())
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name,
                    Selected = d.Id == employee.DepartmentId
                });
                
            return View(employee);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            return NotFound();
        
        ViewBag.Departments = (await _departmentRepository.GetAllAsync())
            .Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
        
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id != employee.Id)
            return BadRequest();
        
        if (ModelState.IsValid)
        {
            await _employeeRepository.UpdateAsync(employee);
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Departments = (await _departmentRepository.GetAllAsync())
            .Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
        
        return View(employee);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            return NotFound();
        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _employeeRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
