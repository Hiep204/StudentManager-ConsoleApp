using NorthWind.Models;
using NorthWind.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Service
{
    internal class EmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;
        public EmployeeService()
        {
            _employeeRepository = new EmployeeRepository();
        }
        public List<NameStatistics> GetEmployeeStatistics()
        {
            return _employeeRepository.GetEmployeeStatistics().Select(c => new NameStatistics
            {
                Name = c.EmployeeName,
                Order = c.TotalOrders,
                Total = c.TotalRevenue,
            }).ToList();
        }
    }
}
