using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
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
                expires: DateTime.Now.AddDays(7),
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

    public async Task<bool> RegisterAdmin(RegisterModel userModel)
    {
        try{
            var userExist = await _userManager.FindByNameAsync(userModel.UserName);
            if(userExist != null)
            {
                return false;
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = userModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if(!result.Succeeded)
            {
                return false;
            }

            if(!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            
            if(!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if(await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            return true;
        }catch(Exception ex){
            Console.WriteLine(ex.ToString());
            return false;
        }      

    }

    public Task<bool> RegisterUser(RegisterModel userModel)
    {
        throw new NotImplementedException();
    }

    public async Task SeedAdmin()
    {
        await RegisterAdmin(new RegisterModel(){
            Email = "useradmin@gmail.com",
            Password = "Clave123*",
            UserName = "useradmin"
        });
    }
}