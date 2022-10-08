namespace WebApi.Models.Users;

using WebApi.Entities;

public class UpdateRequest: User
{
    public string Token { get; set; }
}