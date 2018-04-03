using System.Collections.Generic;
using Bank.Domain.Entities;

namespace Bank.Domain.Abstract
{
    public interface IBankAccountRepository
    {
        IEnumerable<BankAccount> Accounts { get; }
    }
}
