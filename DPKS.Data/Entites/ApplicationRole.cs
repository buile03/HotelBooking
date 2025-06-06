using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Descritption { get; set; }

        public bool IsActive { get; set; } = true;


    }
}
