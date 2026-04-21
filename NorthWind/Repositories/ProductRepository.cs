using NorthWind.DataAccess;
using NorthWind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Repositories
{
    public class ProductRepository
    {
        private readonly NorthWineContext _context;
        public ProductRepository()
        {
            _context = new NorthWineContext();
        }
        public List<ProductStatistics> GetProductStatistics()
        {
            var query = from p in _context.Products
                        join q in _context.OrderDetails on p.ProductId equals q.ProductId
                        group q by new { p.ProductId,
                            p.ProductName
                        } into g
                        select new ProductStatistics
                        {
                            ProductName = g.Key.ProductName,
                            TotalOrders = g.Count(),
                            TotalRevenue = g.Sum(x => x.UnitPrice * x.Quantity)
                        }
            ;

            return query.ToList();
        }
       
    }
}
