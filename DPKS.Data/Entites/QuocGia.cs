using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class QuocGia : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaQG { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Tinh> tinhs { get; set; } = new List<Tinh>();

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
