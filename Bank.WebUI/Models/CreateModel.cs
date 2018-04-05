using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
}