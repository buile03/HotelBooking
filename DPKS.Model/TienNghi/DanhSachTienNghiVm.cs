using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.TienNghi
{
    public class DanhSachTienNghiVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; }
        public bool IsActive { get; set; } = true;

        public string Type { get; set; }
    }
}
