﻿using System.Collections.Generic;
using Bank.Domain.Abstract;
using Bank.Domain.Entities;

namespace Bank.Domain.Concrete
{
    public class EFTransactionsRepositiory: ITransctionsRepository
    {
        private readonly EFDbContext _context = new EFDbContext();
        public IEnumerable<Transaction> Transactions => _context.Transactions;
    }
}
