using Maersk.SCM.Framework.Core.Common;

namespace Maersk.SCM.DeliveryPlanning.Domain.Common
{
    public class LegStatus : Enumeration
    {
        public static LegStatus Created = new LegStatus(0, "Created");
        public static LegStatus Sent = new LegStatus(1, "Sent");
        public static LegStatus Accepted = new LegStatus(2, "Accepted");
        public static LegStatus Rejected = new LegStatus(3, "Rejected");
        public static LegStatus Cancelled = new LegStatus(4, "Cancelled");
        public static LegStatus Updated = new LegStatus(5, "Updated");

        public LegStatus(int id, string name) : base(id, name)
        {
        }
    }
}
