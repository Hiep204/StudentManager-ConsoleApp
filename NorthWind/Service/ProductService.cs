using NorthWind.Models;
using NorthWind.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Service
{
    internal class ProductService
    {
        private readonly ProductRepository _productRepository;
        public ProductService()
        {
            _productRepository = new ProductRepository();
        }
        public List<NameStatistics> GetProductStatistics()
        {
            return _productRepository.GetProductStatistics().Select(p => new NameStatistics
            {
                Name = p.ProductName,
                Order = p.TotalOrders,
                Total = p.TotalRevenue
            }).ToList();
        }
    }
}
