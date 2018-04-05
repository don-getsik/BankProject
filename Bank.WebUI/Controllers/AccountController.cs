using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bank.Domain.Abstract;
using Bank.Domain.Entities;
using Bank.WebUI.Infrastructure;
using Bank.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;


namespace Bank.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ITransctionsRepository _transctions;
        private BankAccountMenager BankManager => HttpContext.GetOwinContext().GetUserManager<BankAccountMenager>();

        public AccountController(ITransctionsRepository transctions)
        {
            _transctions = transctions;
        }

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

        public ViewResult History()
        {
            return View(_transctions.Transactions);
        }

        [HttpPost]
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
            AddErrorsFromResult(result);

            return View(model);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors) ModelState.AddModelError("",error);
        }

    }
}