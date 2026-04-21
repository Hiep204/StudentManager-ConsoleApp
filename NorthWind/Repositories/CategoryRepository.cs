using NorthWind.DataAccess;
using NorthWind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Repositories
{
   
    internal class CategoryRepository
    {

        private readonly NorthWineContext _context;
        public CategoryRepository()
        {
            _context = new NorthWineContext();
        }
        public List<CategoriesStatistics> GetCategoriesStatistics()
        {
            var queryCategories = from c in _context.Categories
                                  join p in _context.Products on c.CategoryId equals p.CategoryId
                                  join q in _context.OrderDetails on p.ProductId equals q.ProductId
                                  group q by new
                                  {
                                      c.CategoryId,
                                      c.CategoryName
                                  } into g
                                  select new CategoriesStatistics
                                  {
                                      CategoryName = g.Key.CategoryName,
                                      TotalOrders = g.Count(),
                                      TotalRevenue = g.Sum(x => x.UnitPrice * x.Quantity)
                                  };
            return queryCategories.ToList();
        }
    }
}
