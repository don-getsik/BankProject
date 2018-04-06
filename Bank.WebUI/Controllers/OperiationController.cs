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
            return BankManager.Users.Single(u => u.Id == id);
        }
        private readonly ITransctionsRepository _transctions;
        private BankAccountMenager BankManager => HttpContext.GetOwinContext().GetUserManager<BankAccountMenager>();

        public OperiationController(ITransctionsRepository transctions)
        {
            _transctions = transctions;
        }

        public ViewResult History()
        {
            return View(_transctions.Transactions);
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
            if (!ModelState.IsValid) return View(model);
            var transaction = new Transaction
            {
                Recesiver = "0",
                Sender = GetAccount().Id,
                Amount = model.amount,
                Date = DateTime.Now.Date
            };
            _transctions.SaveTransaction(transaction);
            await UpdateAmount(model.amount, "this", false);
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
            var transaction = new Transaction
            {
                Recesiver = GetAccount().Id,
                Sender = "0",
                Amount = model.amount,
                Date = DateTime.Now.Date
            };
            _transctions.SaveTransaction(transaction);
            await UpdateAmount(model.amount, "this", true);
            return RedirectToAction("Index", "Account");
        }

        private async Task UpdateAmount(decimal amount, string id, bool add)
        {
            var account = GetAccount(id);
            if(add) account.Balance += amount;
            else account.Balance -= amount;
            await BankManager.UpdateAsync(account);
        }

        public ActionResult Transfer()
        {
            ViewBag.Balance = GetAccount().Balance;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Transfer(TransferModel model)
        {
            if (!ModelState.IsValid && GetAccount(model.recesiver) != null) return View(model);
            var transaction = new Transaction
            {
                Recesiver = GetAccount(model.recesiver).Id,
                Sender = GetAccount().Id,
                Amount = model.amount,
                Date = DateTime.Now.Date
            };
            _transctions.SaveTransaction(transaction);
            await UpdateAmount(model.amount, "this", false);
            await UpdateAmount(model.amount, model.recesiver, true);
            return RedirectToAction("Index", "Account");
        }
    }
}