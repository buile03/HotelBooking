using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Organizations
{
    public class OrganizationVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public bool IsStatus { get; set; }
        
    }
}
