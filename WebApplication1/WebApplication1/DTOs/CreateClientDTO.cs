using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class CreateClientDTO
{
    [Required]
    [MaxLength(120)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(120)]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    [MaxLength(120)]
    public string Email { get; set; }
    [Required]
    [Phone]
    [MaxLength(120)]
    public string Telephone { get; set; }
    [Required]
    [MaxLength(120)]
    public string Pesel { get; set; }
}