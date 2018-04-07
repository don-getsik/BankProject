using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bank.Domain.Entities;

namespace Bank.WebUI.Models
{
    public class HistoryViewModel
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public string CurrentCategory { get; set; }
    }
}