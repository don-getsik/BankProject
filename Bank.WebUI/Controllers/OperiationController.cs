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
            ViewBag.Id = GetAccount().Id;
            var transactions = _transctions.Transactions
                .Where(t => t.Recesiver == ViewBag.Id || t.Sender == ViewBag.Id);
            return View(transactions);
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
            MakeTransaction(model.Amount, sender: GetAccount().Id);
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
            MakeTransaction(model.Amount, recesiver: GetAccount().Id);
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
            if (!ModelState.IsValid && GetAccount(model.Recesiver) != null) return View(model);
            MakeTransaction(model.Amount, model.Recesiver, GetAccount().Id);
            await UpdateAmount(model.Amount, "this", false);
            await UpdateAmount(model.Amount, model.Recesiver, true);
            return RedirectToAction("Index", "Account");
        }

        private void MakeTransaction(decimal amount, string recesiver = "0", string sender = "0")
        {
            var transaction = new Transaction
            {
                Recesiver = recesiver,
                Sender = sender,
                Amount = amount,
                Date = DateTime.Now.Date
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