using System.Collections.Generic;
using Bank.Domain.Entities;

namespace Bank.Domain.Abstract
{
    public interface ITransctionsRepository
    {
        IEnumerable<Transaction> Transactions { get; }
    }
}
