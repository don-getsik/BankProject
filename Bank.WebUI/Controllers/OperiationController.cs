using System;
using System.Collections.Generic;
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
        private BankAccount GetAccount()
        {
            var id = HttpContext.User.Identity.GetUserId();
            return BankManager.Users.Single(u => u.Id == id);
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
            if (!ModelState.IsValid) return View(model);
            var transaction = new Transaction
            {
                Recesiver = "0",
                Sender = GetAccount().Id,
                Amount = model.amount,
                Date = DateTime.Now.Date
            };
            _transctions.SaveTransaction(transaction);
            BankAccount account = GetAccount();
            account.Balance -= model.amount;
            await BankManager.UpdateAsync(account);
            TempData["mesage"] = "Wypłata dokonana prawidłowo";
            return RedirectToAction("Index", "Account");

        }
    }
}