
namespace DPKS.Common.Enum
{
    public enum enLoaiGiuong
    {
        /// <summary>Giường đơn (1 người)</summary>
        DON = 1,

        /// <summary>Giường đôi (2 người)</summary>
        DOI = 2,

        /// <summary>Giường queen (kích thước lớn hơn đôi)</summary>
        QUEEN = 3,

        /// <summary>Giường king (kích thước lớn nhất)</summary>
        KING = 4,

        /// <summary>Giường tầng (thường dùng cho phòng tập thể)</summary>
        TANG = 5,

        /// <summary>Nhiều giường (2 hoặc nhiều giường đơn/đôi)</summary>
        NHIEU_GIUONG = 6,

        /// <summary>Giường sofa (sofa có thể chuyển thành giường)</summary>
        SOFA = 7
    }
}
