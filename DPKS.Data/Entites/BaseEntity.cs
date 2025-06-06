using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class BaseEntity
    {
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LateModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
