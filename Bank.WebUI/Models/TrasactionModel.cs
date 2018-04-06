using System.ComponentModel.DataAnnotations;

namespace Bank.WebUI.Models
{
    public class WithDrawModel
    {
        [Required] public decimal Amount { get; set; }
    }

    public class TransferModel
    {
        [Required] public decimal Amount { get; set; }
        [Required] public string Recesiver { get; set; }
    }
}