using System.ComponentModel.DataAnnotations;
namespace EMS.Models;
public class Department
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Departman adı zorunludur")]
    public string? Name { get; set; }
}
