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
    public class OperiationController : Controller
    {
        private readonly BankAccount _account;
        private readonly ITransctionsRepository _transctions;
        private BankAccountMenager BankManager => HttpContext.GetOwinContext().GetUserManager<BankAccountMenager>();

        public OperiationController(ITransctionsRepository transctions)
        {
//            var owinContext = HttpContext.GetOwinContext();
//            var authManager = owinContext.Authentication;
//            var user = authManager.User;
//            var iden = user.Identity;
            var id = HttpContext.User.Identity.GetUserId();
            _account = BankManager.Users.Single(u => u.Id == id);
            _transctions = transctions;
        }

        // GET: Operiation
        public ActionResult Withdraw()
        {
            var model = new WithDrawModel();
            ViewBag.Balance = _account.Balance;
            return View(model);
        }

        [HttpPost]
        public ActionResult WithDraw(WithDrawModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var transaction = new Transaction
            {
                Recesiver = "0",
                Sender = _account.Id,
                Amount = model.amount,
                Date = DateTime.Now.Date
            };
            _transctions.SaveTransaction(transaction);
            _account.Balance -= model.amount;
            TempData["mesage"] = "Wypłata dokonana prawidłowo";
            return RedirectToAction("Index", "Account");

        }
    }
}