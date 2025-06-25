using Microsoft.AspNetCore.Identity;

namespace DPKS.Data.Entites
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Descritption { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

    }
}
