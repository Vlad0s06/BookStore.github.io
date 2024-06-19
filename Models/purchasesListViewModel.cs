using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Models
{
    public class PurchasesListViewModel
    {
        public IUserRepository userRepository;
        public IOrderProcessorDb purchaseRepository;
        public IBookRepository bookRepository;
        public IDeliveryInfoRepository deliveryInfoRepository;
        public Book Book;
        public int Quantity;
        public DeliveryInfo DeliveryInfo;
        public User User;
        public PurchasesListViewModel(Book book, int quantity, DeliveryInfo deliveryInfo, User user)
        {
            Book = book;
            Quantity = quantity;
            DeliveryInfo = deliveryInfo;
            User = user;
        }
    }
}