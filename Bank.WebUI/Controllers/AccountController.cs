using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bank.Domain.Entities;
using Bank.WebUI.Infrastructure;
using Bank.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;


namespace Bank.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {


            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var account = await BankManager.FindAsync(details.UserName, details.Password);
                if (account == null) ModelState.AddModelError("", "Invalid name or password");
                else
                {
                    var ident = await BankManager.CreateIdentityAsync(account,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties{IsPersistent = false}, ident);
                    return Redirect(returnUrl);
                }
            }
            return View(details);
        }

        private IAuthenticationManager AuthManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Index()
        {
            return View (BankManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                var account = new BankAccount
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Pesel = model.Pesel
                };
                var result = await BankManager.CreateAsync(account, model.Password);
                if (result.Succeeded) return RedirectToAction("Index");
                else AddErrorsFromResult(result);
                }

            return View(model);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("",error);
            }
        }

        private BankAccountMenager BankManager => HttpContext.GetOwinContext().GetUserManager<BankAccountMenager>();
    }
}