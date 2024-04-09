using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabel.Model
{
    public class Employee
    {
        
        

        public int? EmployeeID { get; set; }
        public int? OrganizationID { get; set; }
        public Organization Organization { get; set; }
        public int? DivisionID { get; set; }
        public Division Division { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string? JobTitle { get; set; }


       
    }
}
