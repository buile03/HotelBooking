using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class Tinh : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuocGiaId { get; set; }
        public bool IsActive { get; set; } = true;
        public QuocGia QuocGia { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
