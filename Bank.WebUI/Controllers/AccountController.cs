using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bank.Domain.Entities;
using Bank.WebUI.Infrastructure;
using Bank.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Bank.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private BankAccountMenager BankManager => HttpContext.GetOwinContext().GetUserManager<BankAccountMenager>();

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            bool val = (System.Web.HttpContext.Current.User != null) &&
                       System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (val) return Redirect("Index");
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string button, LoginModel details, string returnUrl)
        {
            if (!ModelState.IsValid) return View(details);
            var account = await BankManager.FindAsync(details.UserName, details.Password);
            if (account == null) ModelState.AddModelError("", "Invalid name or password");
            else
            {
                var ident = await BankManager.CreateIdentityAsync(account,
                    DefaultAuthenticationTypes.ApplicationCookie);
                AuthManager.SignOut();
                AuthManager.SignIn(new AuthenticationProperties{IsPersistent = false}, ident);
                
                return Redirect("Index");
            }

            ViewBag.returnUrl = returnUrl;
            return View(details);
        }

        private IAuthenticationManager AuthManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Index()
        {
            var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            var user = BankManager.Users.Single(u => u.Id == id);
            return View (user);
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var account = new BankAccount
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Pesel = model.Pesel
            };
            var result = await BankManager.CreateAsync(account, model.Password);
            if (result.Succeeded) return RedirectToAction("Login");
            foreach (var error in result.Errors) ModelState.AddModelError("", error);

            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            var result = await BankManager.ChangePasswordAsync(id, model.OldPass, model.NewPass);
            if(result.Succeeded) return Redirect("Index");
            foreach (var error in result.Errors) ModelState.AddModelError("", error);
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return Redirect("Login");
        }
    }
}