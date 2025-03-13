public interface IUserService
{
    Task<bool> RegisterAdmin(RegisterModel userModel);
    Task<bool> RegisterUser(RegisterModel userModel);
    Task<TokenResponse> Login (LoginModel loginModel);
    Task SeedAdmin();
}