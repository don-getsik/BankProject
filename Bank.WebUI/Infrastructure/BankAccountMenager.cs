using Bank.Domain.Concrete;
using Bank.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Bank.WebUI.Infrastructure
{
    public class BankAccountMenager: UserManager<BankAccount>
    {
        public BankAccountMenager(IUserStore<BankAccount> store) : base(store)
        {
        }

        public static BankAccountMenager Create(IdentityFactoryOptions<BankAccountMenager> options,
            IOwinContext context)
        {
            var db = context.Get<BankAccountIdentityDbContext>();
            var menager = new BankAccountMenager(new UserStore<BankAccount>(db));

            return menager;
        }
    }
}