using Microsoft.AspNetCore.Identity;
using Mono.TextTemplating;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        public bool IsActive { get; set; } = false;

        public string CreatedBy { get; set; }


        public string? ModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int QuocGiaId { get; set; }

        public int TinhId { get; set; }

        public string? EmailConfirmationCode { get; set; }
        public DateTime? ConfirmationCodeExpiry { get; set; }

        public string? ResetPasswordCode { get; set; }
        public DateTime? ResetPasswordCodeExpiry { get; set; }

        public string? TwoFactorCode { get; set; }
        public DateTime? TwoFactorCodeExpiry { get; set; }

        public string PhotoName { get; set; }

        public QuocGia QuocGia { get; set; }

        public Tinh Tinh { get; set; }

        public ICollection<DatPhong> datPhongs { get; set; } = new List<DatPhong>();

        public ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();
    }

}
