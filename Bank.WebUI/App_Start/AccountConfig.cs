using Bank.Domain.Concrete;
using Bank.WebUI.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Bank.WebUI
{
    public class AccountConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(BankAccountIdentityDbContext.Create);
            app.CreatePerOwinContext<BankAccountMenager>(BankAccountMenager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }
    }
}