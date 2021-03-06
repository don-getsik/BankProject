﻿using System;
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
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
    }
}
