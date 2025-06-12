namespace DPKS.Data.Entites
{
    public class TienNghiTheoLoaiPhong
    {
        
        public int LoaiPhongId { get; set; }
        public int TienNghiId { get; set; }

        //Navigation Properties 

        public LoaiPhong LoaiPhong { get; set; }

        public TienNghi TienNghi { get; set; }
    }
}
