using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.User
{
    public class UserVm
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string? PhotoName { get; set; }

        public string? QuocGia { get; set; }

        public string? Tinh { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLogin { get; set; }

        public List<string> Roles { get; set; } = new();
    }

}
