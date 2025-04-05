using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public List<ApplicationUser> GetAllUsers()
    {
        try
        {
            var users = _userManager.Users.ToList();
            return users;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener los usuarios: {ex.Message}");
            return new List<ApplicationUser>();
        }
    }

    public async Task<TokenResponse> Login(LoginModel loginModel)
    {
        var user = await _userManager.FindByNameAsync(loginModel.UserName);
        if(user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTConfig:SecretKey"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWTConfig:ValidIssuer"],
                audience: _configuration["JWTConfig:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
            );

            return new TokenResponse(){
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                UserName = user.UserName
            };
        }
        return new TokenResponse();
    }

    public async Task<bool> RegisterUser(RegisterUserDTO userModel)
    {
        try
        {
            var userExist = await _userManager.FindByNameAsync(userModel.UserName);
            if (userExist != null)
                return false;

            var user = new ApplicationUser
            {
                Email = userModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
                return false;

            // Asegurar que los roles existen
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            // Asignar el rol según el parámetro
            var role = userModel.IsAdmin ? UserRoles.Admin : UserRoles.User;
            await _userManager.AddToRoleAsync(user, role);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al registrar usuario: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ResetPasswordUser(ResetPasswordDTO resetPasswordDTO)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.emailOrUserName) ??
                    await _userManager.FindByNameAsync(resetPasswordDTO.emailOrUserName);

            if (user == null)
                return false;

            // Generar token de reseteo
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Resetear contraseña con la proporcionada
            var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordDTO.newPassword);

            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al reiniciar clave: {ex.Message}");
            return false;
        }
    }

    public async Task SeedAdmin()
    {
        await RegisterUser(new RegisterUserDTO(){
            Email = "userCristian@gmail.com",
            Password = "Clave123*",
            UserName = "CristianZuluaga",
            IsAdmin = true
        });
    }

    public async Task<bool> UpdateStatusUser(RequestUpdateStatusUserDTO requestUpdateStatusUserDTO)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(requestUpdateStatusUserDTO.IdUser);
            if (user == null)
                return false;

            user.IsActive = requestUpdateStatusUserDTO.IsActive;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al inactivar usuario: {ex.Message}");
            return false;
        }
    }
}