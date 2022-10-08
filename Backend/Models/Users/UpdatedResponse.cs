namespace WebApi.Models.Users;

using WebApi.Entities;

public class UpdatedResponse
{
    public UpdatedResponse(string message )
    {
        Message = message;  
    }

    public string Message { get; set; }

   
}