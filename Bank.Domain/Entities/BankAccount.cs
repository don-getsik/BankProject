using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Domain.Entities
{
    [Table("BankAccount")]
    public class BankAccount
    {

        [Key]
        public int IdAccount { get; set; }
        public string Login { get; set; }
        public double Balance { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public uint Pesel { get; set; }
        public string Password { get; set; }

        public bool CheckLogin(string login, string pass)
        {
            return Login.Equals(login) && Password.Equals(pass);
        }

        public bool ChangePassword(string old, string pass)
        {
            if (!Password.Equals(old)) return false;
            Password = pass;
            return true;
        }

        public bool Transaction(BankAccount recesiver, float amount)
        {
            if (Balance < amount) return false;
            Balance -= amount;
            recesiver.Balance += amount;
            return true;
        }
    }
}
