using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Bank.Domain.Entities;

namespace Bank.Domain.Concrete
{
    public class BankAccountIdentityDbContext: IdentityDbContext<BankAccount>
    {
        public BankAccountIdentityDbContext() : base("BankDb")
        {
        }

        static BankAccountIdentityDbContext()
        {
            Database.SetInitializer(new BankDbInit());
        }
        public static BankAccountIdentityDbContext Create()
        {
            return new BankAccountIdentityDbContext();
        }
    }

    public class BankDbInit : DropCreateDatabaseIfModelChanges<BankAccountIdentityDbContext>
    {
        protected override void Seed(BankAccountIdentityDbContext context)
        {
            PerformInitalSetup(context);
            base.Seed(context);
        }

        public void PerformInitalSetup(BankAccountIdentityDbContext context)
        {

        }
    }
}
