namespace WebApi.Models.Users;

using System.ComponentModel.DataAnnotations;

public class RegisterUserRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Password { get; set; }
}