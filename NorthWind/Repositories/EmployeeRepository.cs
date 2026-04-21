using NorthWind.DataAccess;
using NorthWind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Repositories
{
    internal class EmployeeRepository
    {
        private readonly NorthWineContext _context;
         public EmployeeRepository()
        {
            _context = new NorthWineContext();
        }
        public List<EmployeeStatistics> GetEmployeeStatistics()
        {
            var query  = from e in _context.Employees
                         join o in _context.Orders on e.EmployeeId equals o.EmployeeId
                         join c in _context.OrderDetails on o.OrderId equals c.OrderId
                         group c  by new { e.FirstName, e.LastName } into g
                         select new EmployeeStatistics
                         {
                             EmployeeName = g.Key.FirstName + " " + g.Key.LastName,
                             TotalOrders = g.Select(x => x.OrderId).Distinct().Count(),
                             TotalRevenue = g.Sum(x => x.UnitPrice * x.Quantity)
                         };
            return query.ToList();
        }
    }
}
