using Maersk.SCM.Framework.Core.Common;

namespace Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate
{
    public class BookingStatus : Enumeration
    {
        public static BookingStatus Created = new BookingStatus(0, "Created");
        public static BookingStatus Updated = new BookingStatus(1, "Updated");
        public static BookingStatus Cancelled = new BookingStatus(2, "Cancelled");

        public BookingStatus(int id, string name) : base(id, name)
        {
        }
    }
}
