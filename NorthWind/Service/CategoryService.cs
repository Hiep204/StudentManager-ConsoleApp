using NorthWind.Models;
using NorthWind.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Service
{
    
    internal class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        public CategoryService()
        {
            _categoryRepository = new CategoryRepository();
        }
        public List<NameStatistics> GetCategoriesStaistics()
        {
            return _categoryRepository.GetCategoriesStatistics().Select(c => new NameStatistics
            {
                Name = c.CategoryName,
                Order = c.TotalOrders,
                Total = c.TotalRevenue,            
            }).ToList();
        }
    }
}
