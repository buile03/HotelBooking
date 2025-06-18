using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.User
{
    public class UpdateProfileVm
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public int QuocGiaId { get; set; }

        [Required]
        public int TinhId { get; set; }

        public string? PhotoName { get; set; }

        public string Email { get; set; }
    }
}
