using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Models
{
    public class CategoriesStatistics
    {
        public String CategoryName { get; set; }
       public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}
