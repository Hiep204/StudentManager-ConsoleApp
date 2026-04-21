using NorthWind.Models;
using NorthWind.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Service
{
    internal class CustomerService
    {
        private readonly CustomerRepository _customerRepository;
        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }
        public List<NameStatistics> GetCustomerStatistics()
        {
            return _customerRepository.GetCustomerStatistics().Select(c => new NameStatistics
            {
                Name = c.CustomerName,
                Order = c.TotalOrders,
                Total = c.TotalRevenue,
            }).ToList();
        }
    }
}
