using BookStore.Domain.Entities;
using BookStore.Domain.Abstract;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace BookStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public IUserRepository userRepository;
        public IOrderProcessorDb purchaseRepository;
        public IBookRepository bookRepository;
        public IDeliveryInfoRepository deliveryInfoRepository;
        public AccountController(IUserRepository userRepo, IOrderProcessorDb purchaseRepo,
            IBookRepository bookRepo, IDeliveryInfoRepository deliveryInfoRepo)
        {
            userRepository = userRepo;
            purchaseRepository = purchaseRepo;
            bookRepository = bookRepo;
            deliveryInfoRepository = deliveryInfoRepo;
        }
        public ViewResult ProfileInfoChanges() => View();
        public ActionResult Index()
        {
            var user = userRepository.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            return View(user);
        }
        public ViewResult EditProfile(int userId)
        {
            var user = userRepository.Users.FirstOrDefault(u => u.UserId == userId);
            return View(user);
        }
        [HttpPost]
        public ActionResult EditProfile(User user)
        {
            if (ModelState.IsValid)
            {
                userRepository.SaveUser(user);
                TempData["message"] = string.Format("Изменения в профиле были сохранены");
                FormsAuthentication.SignOut();
                return RedirectToAction("ProfileInfoChanges");
            }
            else
                return View(user);
        }
        public ViewResult EditPassword(int userId)
        {
            var user = userRepository.Users.FirstOrDefault(u => u.UserId == userId);
            return View(user);
        }
        [HttpPost]
        public ActionResult EditPassword(int userId, string oldPass, string newPass)
        {
            var user = userRepository.Users.FirstOrDefault(u => u.UserId == userId);
            if (ModelState.IsValid)
            {
                if (newPass == "")
                    ModelState.AddModelError("", "Вы не ввели новый пароль. Изменение невозможно.");
                else if (oldPass == newPass)
                    ModelState.AddModelError("", "Текущий и новый пароли совпадают. Изменение невозможно.");
                else if (user.Password == oldPass)
                {
                    userRepository.ChangePassword(user, newPass);
                    TempData["message"] = string.Format("Изменения в профиле были сохранены");
                    FormsAuthentication.SignOut();
                    return RedirectToAction("ProfileInfoChanges");
                }
                else
                    ModelState.AddModelError("", "Вы ввели неверный текущий пароль. Изменение невозможно.");
                return View(user);
            }
            else
                return View(user);
        }
        public ViewResult Purchases(int? userId)
        {
            var purchases = purchaseRepository.Purchases.Where(p => p.UserId == userId);
            return View(purchases);
        }
        public ActionResult PurchaseDetails(int deliveryInfoId)
        {
            var purchases = purchaseRepository.Purchases.Where(p => p.DeliveryInfoId == deliveryInfoId);
            return PartialView(purchases);
        }
        public ActionResult DeliveryInfo(int deliveryInfoId)
        {
            var deliveryInfo = deliveryInfoRepository.DeliveryInfo.Where(d => d.DeliveryInfoId == deliveryInfoId).FirstOrDefault();
            return PartialView(deliveryInfo);
        }
        [HttpPost]
        public RedirectToRouteResult ConfirmReceipt(int deliveryInfoId, string returnUrl, int userId)
        {
            var purchases = purchaseRepository.Purchases.Where(p => p.DeliveryInfoId == deliveryInfoId).ToList();
            if (purchases != null)
                foreach (var p in purchases)
                    purchaseRepository.ChangeDeliveryStatus(p.OrderId, "Получен");
            return RedirectToAction("Purchases", new { returnUrl, userId });
        }
    }
}