using System.Collections.Generic;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Abstract
{
    public interface IDeliveryInfoRepository
    {
        IEnumerable<DeliveryInfo> DeliveryInfo { get; }
        void SaveDeliveryInfo(DeliveryInfo deliveryInfo);
        DeliveryInfo DeleteDeliveryInfo(int orderId);
    }
}
