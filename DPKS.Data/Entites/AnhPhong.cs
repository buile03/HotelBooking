using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class AnhPhong : BaseEntity
    {
        
        public int PhotoId { get; set; } // Primary key for the photo
        public int PhongId { get; set; }
        public string PhotoName { get; set; }


        //navigation property
        public Phong Phong {  get; set; }
    }
}
