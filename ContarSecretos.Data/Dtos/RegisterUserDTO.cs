using System.ComponentModel.DataAnnotations;

public class RegisterUserDTO{
    [Required(ErrorMessage ="User name is required")]
    public string UserName{get;set;} = string.Empty;
    
    [EmailAddress] // usuario @ servidor . dominio
    [Required(ErrorMessage ="Email is required")]
    public string Email {get;set;} = string.Empty;
    
    [Required(ErrorMessage ="Password is required")]
    public string Password {get;set;} = string.Empty;
    public bool IsAdmin {get;set;} = false;
}