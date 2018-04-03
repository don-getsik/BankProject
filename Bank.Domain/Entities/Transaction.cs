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
        public int Recesiver { get; set; }
        public int Sender { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
