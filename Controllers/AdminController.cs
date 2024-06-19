using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Entities;
using BookStore.Domain.Abstract;

namespace BookStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IBookRepository bookRepository;
        public IUserRepository userRepository;
        public IDeliveryInfoRepository deliveryInfoRepository;
        public IOrderProcessorDb purchaseRepository;
        public AdminController(IBookRepository bookRepo, IUserRepository userRepo,
            IOrderProcessorDb purchaseRepo, IDeliveryInfoRepository deliveryInfoRepo)
        {
            bookRepository = bookRepo;
            userRepository = userRepo;
            purchaseRepository = purchaseRepo;
            deliveryInfoRepository = deliveryInfoRepo;
        }
        public ViewResult Index() => View();
        public ActionResult BookList() => PartialView(bookRepository.Books);
        public ActionResult UserList() => PartialView(userRepository.Users);
        public ActionResult PurchaseList() => PartialView(purchaseRepository.Purchases);
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
        public ViewResult Edit(int bookId)
        {
            var book = bookRepository.Books.FirstOrDefault(b => b.BookId == bookId);
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    book.ImageType = image.ContentType;
                    book.Image = new byte[image.ContentLength];
                    image.InputStream.Read(book.Image, 0, image.ContentLength);
                }
                bookRepository.SaveBook(book);
                TempData["message"] = string.Format("Изменения в книге \"{0}\" были сохранены", book.Name);
                return RedirectToAction("Index");
            }
            else
                return View(book);
        }
        public ViewResult Create() => View("Edit", new Book());
        [HttpPost]
        public ActionResult Delete(int bookId)
        {
            var deletedBook = bookRepository.DeleteBook(bookId);
            if (deletedBook != null)
                TempData["message"] = string.Format("Книга \"{0}\" была удалена",
                    deletedBook.Name);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeletePurchase(int deliveryInfoId)
        {
            var purchases = purchaseRepository.Purchases.Where(p => p.DeliveryInfoId == deliveryInfoId).ToList();
            if (purchases != null)
                foreach (var p in purchases)
                {
                    TempData["message"] = string.Format("Заказ был удален");
                    purchaseRepository.DeletePurchase(p.OrderId);
                }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeferPurchase(int deliveryInfoId)
        {
            var purchases = purchaseRepository.Purchases.Where(p => p.DeliveryInfoId == deliveryInfoId).ToList();
            if (purchases != null)
                foreach (var p in purchases)
                    purchaseRepository.ChangeDeliveryStatus(p.OrderId, "Отложен");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult SendPurchase(int deliveryInfoId)
        {
            var purchases = purchaseRepository.Purchases.Where(p => p.DeliveryInfoId == deliveryInfoId).ToList();
            if (purchases != null)
                foreach (var p in purchases)
                    purchaseRepository.ChangeDeliveryStatus(p.OrderId, "Отправлен");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GiveRole(int userId, int roleId)
        {
            var changedUser = userRepository.GiveRole(userId, roleId);
            if (changedUser != null)
                TempData["message"] = string.Format("Роль пользователя \"{0}\" была изменена",
                    changedUser.Email);
            return RedirectToAction("Index");
        }
    }
}