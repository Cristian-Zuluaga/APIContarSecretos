using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser(RegisterUserDTO registerModel)
    {
        var response = await _userService.RegisterUser(registerModel);
        return Ok(response);
    }

    [HttpPost("updateStatusUser")]
    public async Task<IActionResult> UpdateStatusUser(RequestUpdateStatusUserDTO requestUpdateStatusUserDTO)
    {
        var response = await _userService.UpdateStatusUser(requestUpdateStatusUserDTO);
        return Ok(response);
    }

     [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
    {
        var response = await _userService.ResetPasswordUser(resetPasswordDTO);
        return Ok(response);
    }

    [HttpGet("getAll")]
    public IActionResult GetAll(){
        var response = _userService.GetAllUsers();
        return Ok(response);
    }
    
}