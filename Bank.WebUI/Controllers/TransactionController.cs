using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TransactionController : Controller
    {
        private readonly ITransctionsRepository _transctions;
        private BankAccountMenager BankManager => HttpContext.GetOwinContext().GetUserManager<BankAccountMenager>();
        public TransactionController(ITransctionsRepository transctions)
        {
            _transctions = transctions;
        }

        private BankAccount GetAccount(string id = "this")
        {
            if (id == "this") id = HttpContext.User.Identity.GetUserId();
            return BankManager.Users.SingleOrDefault(u => u.Id == id);
        }

        public ViewResult History(string category)
        {
            ViewBag.Id = GetAccount().Id;
            var transactions = _transctions.Transactions
                .Where(t => t.Recesiver == ViewBag.Id || t.Sender == ViewBag.Id);
            var viewModel = new HistoryViewModel
            {
                Transactions = transactions
                    .Where(t => category == null || t.Type == category)
                    .OrderBy(t => t.IdTransaction),
                CurrentCategory = category
            };
            return View(viewModel);
        }
    }
}