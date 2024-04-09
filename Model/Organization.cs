using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tabel.Model
{
    public class Organization
    {
        public int? OrganizationID { get; set; }
        public string? NameOrganization { get; set; }
        public string? Managment { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? House { get; set; }
        public int? Office { get; set; }
        public string? Email { get; set; }
        public ICollection<Employee> Employees { get;}

    }
}
