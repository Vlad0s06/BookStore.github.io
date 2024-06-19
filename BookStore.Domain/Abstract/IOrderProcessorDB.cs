using BookStore.Domain.Entities;
using System.Collections.Generic;

namespace BookStore.Domain.Abstract
{
    public interface IOrderProcessorDb
    {
        IEnumerable<Purchase> Purchases { get; }
        void ProcessOrderDB(Cart cart, DeliveryInfo deliveryInfo, User user);
        void ChangeDeliveryStatus(int orderId, string status);
        void DeletePurchase(int orderId);
    }
}
