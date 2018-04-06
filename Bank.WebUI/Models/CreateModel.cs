using System.ComponentModel.DataAnnotations;

namespace Bank.WebUI.Models
{
    public class CreateModel
    {
        [Required] public string UserName { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Pesel { get; set; }
        [Required] public string Password { get; set; }
    }

    public class LoginModel
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required] public string OldPass { get; set; }
        [Required] public string NewPass { get; set; }
    }
}