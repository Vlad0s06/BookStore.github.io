using System.Collections.Generic;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Concrete
{
    public class EFDeliveryInfoRepository : IDeliveryInfoRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<DeliveryInfo> DeliveryInfo => context.DeliveryInfo;
        public void SaveDeliveryInfo(DeliveryInfo deliveryInfo)
        {
            if (deliveryInfo.DeliveryInfoId == 0)
                context.DeliveryInfo.Add(deliveryInfo);
            else
            {
                var dbEntry = context.DeliveryInfo.Find(deliveryInfo.DeliveryInfoId);
                if (dbEntry != null)
                {
                    dbEntry.FullName = deliveryInfo.FullName;
                    dbEntry.Postcode = deliveryInfo.Postcode;
                    dbEntry.Country = deliveryInfo.Country;
                    dbEntry.City = deliveryInfo.City;
                    dbEntry.Street = deliveryInfo.Street;
                    dbEntry.Building = deliveryInfo.Building;
                    dbEntry.Appartment = deliveryInfo.Appartment;
                }
            }
            context.SaveChanges();
        }
        public DeliveryInfo DeleteDeliveryInfo(int orderId)
        {
            var dbEntry = context.DeliveryInfo.Find(orderId);
            if (dbEntry != null)
            {
                context.DeliveryInfo.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
