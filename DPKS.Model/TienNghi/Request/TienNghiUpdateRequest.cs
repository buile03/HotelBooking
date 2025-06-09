namespace DPKS.Model.TienNghi.Request
{
    public class TienNghiUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // public string ModifiedBy { get; set }
        public DateTime LastModifiedDate => DateTime.Now;
        public bool IsActive { get; set; }
    }
}
