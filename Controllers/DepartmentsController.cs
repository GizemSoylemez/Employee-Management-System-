using EMS.Models;
using EMS.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace EMS.Controllers;
public class DepartmentsController : Controller
{
    private readonly IDepartmentRepository _departmentRepository;
    public DepartmentsController(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }
    public async Task<IActionResult> Index()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return View(departments);
    }
    public async Task<IActionResult> Details(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
            return NotFound();
        return View(department);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department department)
    { 
        if (ModelState.IsValid)
        {
            try
            {
                await _departmentRepository.AddAsync(department);
                TempData["Success"] = "Departman başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Departman eklenirken bir hata oluştu: " + ex.Message);
            }
        }
        return View(department);
    }
    public async Task<IActionResult> Edit(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
            return NotFound();
        return View(department);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Department department)
    {
        if (id != department.Id)
            return BadRequest();
        if (ModelState.IsValid)
        {
            await _departmentRepository.UpdateAsync(department);
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }
    public async Task<IActionResult> Delete(int id)
    {
        try 
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
                return NotFound();
            
            return View(department);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Departman silinirken bir hata oluştu: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _departmentRepository.DeleteAsync(id);
            TempData["Success"] = "Departman başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Departman silinirken bir hata oluştu: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
