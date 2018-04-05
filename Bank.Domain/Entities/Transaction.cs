using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Domain.Entities
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public int IdTransaction { get; set; }
        public string Recesiver { get; set; }
        public string Sender { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
