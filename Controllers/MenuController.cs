using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class MenuController : Controller
    {
        public ActionResult Author() => View();
        public ActionResult Wholesale() => View();
        public ActionResult Cooperation() => View();
        public ActionResult Contacts() => View();
        public ActionResult Support() => View();
    }
}