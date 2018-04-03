using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bank.Domain.Abstract;

namespace Bank.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IBankAccountRepository _accountRepository;

        public AccountController(IBankAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        // GET: Account
        public ViewResult List()
        {
            return View(_accountRepository.Accounts);
        }
    }
}