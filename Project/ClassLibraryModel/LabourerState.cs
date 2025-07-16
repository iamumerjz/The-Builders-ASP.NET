using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryModel
{
    public class LabourerStats
    {
        public int TotalOrders { get; set; }
        public decimal EarnedAmount { get; set; }
        public int CancelledOrders { get; set; }
        public int CompletedOrders { get; set; }
    }

}
