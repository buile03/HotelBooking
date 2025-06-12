namespace DPKS.Common.Enum
{
    public enum enTrangThaiPhong
    {
        /// <summary>Phòng đang trống, sẵn sàng cho khách đặt</summary>
        TRONG = 1,

        /// <summary>Phòng đã được đặt nhưng khách chưa đến</summary>
        DADAT = 2,

        /// <summary>Khách đã check-in và đang ở</summary>
        DANGO = 3,

        /// <summary>Phòng đang được dọn dẹp</summary>
        DANGDONDEP = 4,

        /// <summary>Phòng đang được bảo trì, không thể sử dụng</summary>
        BAOTRI = 5,

        /// <summary>Phòng không khả dụng (lỗi kỹ thuật, khóa phòng,...)</summary>
        KHONGKHADUNG = 6
    }
}
