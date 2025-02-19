using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EMS.Models;
public class Employee
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Ad Soyad alanı zorunludur")]
    public string? FullName { get; set; }
    [Required(ErrorMessage = "E-posta alanı zorunludur")]
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    [Required(ErrorMessage = "Departman seçimi zorunludur")]
    public int DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    public virtual Department? Department { get; set; }
    [Required(ErrorMessage = "İşe başlama tarihi zorunludur")]
    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; }
    [Required(ErrorMessage = "Maaş alanı zorunludur")]
    public decimal Salary { get; set; }
}
