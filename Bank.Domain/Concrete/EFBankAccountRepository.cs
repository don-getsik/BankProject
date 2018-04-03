using System.Collections.Generic;
using Bank.Domain.Abstract;
using Bank.Domain.Entities;

namespace Bank.Domain.Concrete
{
    public class EFBankAccountRepository: IBankAccountRepository
    {
        private readonly EFDbContext _context = new EFDbContext();

        public IEnumerable<BankAccount> Accounts => _context.Accounts;
    }
}
