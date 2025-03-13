using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required(ErrorMessage ="username is required")]
    public string UserName {get;set;} = string.Empty;
    [Required(ErrorMessage ="Password is required")]
    public string Password {get;set;} = string.Empty;
}