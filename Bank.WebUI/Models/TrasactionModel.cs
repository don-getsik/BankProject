using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank.WebUI.Models
{
    public class WithDrawModel
    {
        [Required] public double amount;
    }
}