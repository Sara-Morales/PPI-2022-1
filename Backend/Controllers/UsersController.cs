namespace WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Services;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult Register(RegisterUserRequest model)
    {
        var response = _userService.Register(model);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult LoginUser(LoginUserRequest model)
    {
        var response = _userService.LoginUser(model);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);
        return Ok(response);
    }


    [AllowAnonymous]
    [HttpPut("[action]")]
    public IActionResult UpdateUser(UpdateRequest model)
    {
        var response = _userService.UpdateUser(model);
        return Ok(response);
    }



    [Authorize(Role.Admin)]
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        // only admins can access other user records
        var currentUser = (User)HttpContext.Items["User"];
        if (id != currentUser.Id && currentUser.Role != Role.Admin)
            return Unauthorized(new { message = "Unauthorized" });

        var user =  _userService.GetById(id);
        return Ok(user);
    }
}