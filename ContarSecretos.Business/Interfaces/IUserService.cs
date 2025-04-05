public interface IUserService
{
    Task<bool> RegisterUser(RegisterUserDTO userModel);
    Task<TokenResponse> Login (LoginModel loginModel);
    List<ApplicationUser> GetAllUsers();
    Task<bool> UpdateStatusUser(RequestUpdateStatusUserDTO requestUpdateStatusUserDTO);
    Task<bool> ResetPasswordUser(ResetPasswordDTO resetPasswordDTO);
    Task SeedAdmin();
}