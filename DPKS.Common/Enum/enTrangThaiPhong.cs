namespace DPKS.Common.Enum
{
    public enum enTrangThaiPhong
    {
        /// <summary>Phòng đang trống, sẵn sàng cho khách đặt</summary>
        TRONG = 0,

        /// <summary>Phòng đã được đặt nhưng khách chưa đến</summary>
        DADAT = 1,

        /// <summary>Khách đã check-in và đang ở</summary>
        DANGO = 2,

        /// <summary>Phòng đang được dọn dẹp</summary>
        DANGDONDEP = 3,

        /// <summary>Phòng đang được bảo trì, không thể sử dụng</summary>
        BAOTRI = 4,

        /// <summary>Phòng không khả dụng (lỗi kỹ thuật, khóa phòng,...)</summary>
        KHONGKHADUNG = 5
    }
}
