using System.ComponentModel.DataAnnotations;

namespace Bank.WebUI.Models
{
    public class WithDrawModel
    {
        [Required] [Range(1,int.MaxValue, ErrorMessage = "Proszę podac kwotę większą niż 1zł")] public decimal Amount { get; set; }
    }

    public class TransferModel
    {
        [Required] [Range(1, int.MaxValue, ErrorMessage = "Proszę podac kwotę większą niż 1zł")] public decimal Amount { get; set; }
        [Required] public string Recesiver { get; set; }
    }
}