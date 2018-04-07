using System;
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

namespace Bank.WebUI.Controllers
{
    [Authorize]
    public class OperiationController : Controller
    {
        private BankAccount GetAccount(string id = "this")
        {
            if(id == "this") id = HttpContext.User.Identity.GetUserId();
            return BankManager.Users.SingleOrDefault(u => u.Id == id);
        }
        private readonly ITransctionsRepository _transctions;
        private BankAccountMenager BankManager => HttpContext.GetOwinContext().GetUserManager<BankAccountMenager>();

        public OperiationController(ITransctionsRepository transctions)
        {
            _transctions = transctions;
        }

        // GET: Operiation
        public ActionResult Withdraw()
        {
            ViewBag.Balance = GetAccount().Balance;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Withdraw(WithDrawModel model)
        {
            if (model.Amount > GetAccount().Balance) ModelState.AddModelError(string.Empty, "Podałeś kwotę większą niż posiadasz na koncie");
            if (!ModelState.IsValid) return View(model);
            MakeTransaction(model.Amount,"Withdraw", sender: GetAccount().Id);
            await UpdateAmount(model.Amount, "this", false);
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Deposit(WithDrawModel model)
        {
            if (!ModelState.IsValid) return View(model);
            MakeTransaction(model.Amount,"Deposit", recesiver: GetAccount().Id);
            await UpdateAmount(model.Amount, "this", true);
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Transfer()
        {
            ViewBag.Balance = GetAccount().Balance;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Transfer(TransferModel model)
        {
            if (model.Amount > GetAccount().Balance) ModelState.AddModelError(string.Empty, "Podałeś kwotę większą niż posiadasz na koncie");
            if (GetAccount(model.Recesiver) == null) ModelState.AddModelError(string.Empty, "Podano nieprawidłowy numer konta");
            if (!ModelState.IsValid) return View(model);
            MakeTransaction(model.Amount,"Transfer", recesiver: model.Recesiver, sender: GetAccount().Id);
            await UpdateAmount(model.Amount, "this", false);
            await UpdateAmount(model.Amount, model.Recesiver, true);
            return RedirectToAction("Index", "Account");
        }

        private void MakeTransaction(decimal amount, string type, string recesiver = "0", string sender = "0")
        {
            var transaction = new Transaction
            {
                Recesiver = recesiver,
                Sender = sender,
                Amount = amount,
                Date = DateTime.Now.Date,
                Type = type
            };
            _transctions.SaveTransaction(transaction);
        }
        private async Task UpdateAmount(decimal amount, string id, bool add)
        {
            var account = GetAccount(id);
            if (add) account.Balance += amount;
            else account.Balance -= amount;
            await BankManager.UpdateAsync(account);
        }
    }
}