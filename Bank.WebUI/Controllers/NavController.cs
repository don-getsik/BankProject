using System.Collections.Generic;
using System.Web.Mvc;
using Bank.Domain.Abstract;
using System.Linq;

namespace SportsStore.WebUI.Controllers
{

    public class NavController : Controller
    {
        private readonly ITransctionsRepository _repository;

        public NavController(ITransctionsRepository repo)
        {
            _repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {

            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = _repository.Transactions
                .Select(x => x.Type)
                .Distinct()
                .OrderBy(x => x);

            return PartialView(categories);
        }
    }
}
