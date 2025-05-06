using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class CreateClientDTO
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Phone]
    public string Telephone { get; set; }
    [Required]
    [Length(11,11)]
    public string Pesel { get; set; }
}