namespace DPKS.Data.Entites
{
    public class AnhPhong : BaseEntity
    {
        
        public int PhotoId { get; set; } 
        public int PhongId { get; set; }
        public string PhotoName { get; set; }


        //navigation property
        public Phong Phong {  get; set; }
    }
}
