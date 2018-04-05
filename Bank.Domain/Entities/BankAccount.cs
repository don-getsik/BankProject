using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bank.Domain.Entities
{
    [Table("BankAccount")]
    public class BankAccount : IdentityUser
    {
        [Key]
        public override string Id { get; set; }
        public override string UserName { get; set; }
        public decimal Balance { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }
    }
}
