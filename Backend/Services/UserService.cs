namespace WebApi.Services;

using BCrypt.Net;
using Microsoft.Extensions.Options;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;
using System.Linq;

public interface IUserService
{
    AuthenticateResponse Authenticate(AuthenticateRequest model);

    AuthenticateResponse LoginUser(LoginUserRequest model);
    IEnumerable<User> GetAll();
    User GetById(int id);
    AuthenticateResponse Register(RegisterUserRequest model);
    UpdatedResponse UpdateUser(UpdateRequest model);
}

public class UserService : IUserService
{
    private DataContext _context;
    private IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;

    public UserService(
        DataContext context,
        IJwtUtils jwtUtils,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
    }


    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        // authentication successful so generate jwt token
        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new AuthenticateResponse(user, jwtToken);
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User GetById(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }


    public AuthenticateResponse Register(RegisterUserRequest model)
    {
        IEnumerable<User> userList = _context.Users;
        var idNumberList = userList.Select(s => s.Id);
        var idNumber = idNumberList.Max() + 1;
        var newUser = new User { Id = idNumber, FirstName = model.Name, Email = model.Email, LastName = "", PasswordHash = BCrypt.HashPassword(model.Password), Role = Role.User };
        _context.Users.AddRange(newUser);
        _context.SaveChanges();
        var jwtToken = _jwtUtils.GenerateJwtToken(newUser);
        return new AuthenticateResponse(newUser, jwtToken);
    }

    public AuthenticateResponse LoginUser(LoginUserRequest model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        // authentication successful so generate jwt token
        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new AuthenticateResponse(user, jwtToken);

    }

    public UpdatedResponse UpdateUser(UpdateRequest model)
    {

        var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);
        var authenticatedUserID = _jwtUtils.ValidateJwtToken(model.Token);
        if (authenticatedUserID == null)
           {
            throw new AppException("No authenticado");
          }
        user.Username = model.Username;
        user.LastName = model.LastName;        
        user.FirstName = model.FirstName;
        _context.SaveChanges();
        return new UpdatedResponse("todo bien mompirri");
    }
}

