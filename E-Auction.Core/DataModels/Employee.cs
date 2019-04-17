using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Auction.Core.DataModels
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DoB { get; set; }
        public string Email { get; set; }

        public int EmployeePositionId { get; set; }
        public virtual EmployeePosition EmployeePosition { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }

    public class EmployeePosition
    {
        public int Id { get; set; }
        public string PositionName { get; set; }

        public ICollection<Employee> Employees { get; set; }
        public EmployeePosition()
        {
            Employees = new List<Employee>();
        }
    }
}
