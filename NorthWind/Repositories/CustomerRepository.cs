using NorthWind.DataAccess;
using NorthWind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Repositories
{
    internal class CustomerRepository
    {
        private readonly NorthWineContext _context;
        public CustomerRepository()
        {
            _context = new NorthWineContext();
        }
        public List<CustomerStatistics> GetCustomerStatistics()
        {
            var query = from o in _context.Customers
                        join p in _context.Orders on o.CustomerId equals p.CustomerId
                        join q in _context.OrderDetails on p.OrderId equals q.OrderId
                        group q by new
                        {
                            o.CustomerId,
                            o.CompanyName
                        } into g
                        select new CustomerStatistics
                        {
                            CustomerName = g.Key.CompanyName,
                            TotalOrders = g.Select(x => x.OrderId).Distinct().Count(),
                            TotalRevenue = g.Sum(x => x.UnitPrice * x.Quantity)
                        };
            return query.ToList();
        }
    }
}
