using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DPKS.Common.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;


public static class enHelper
{
    public static string GetDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        if (field != null)
        {
            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            if (attr != null)
            {
                return attr.Description;
            }
        }

        return value.ToString(); // fallback
    }
    public static List<SelectListItem> GetSelectListPhuongThuc()
    {
        return Enum.GetValues(typeof(enLoaiThanhToan))
            .Cast<enLoaiThanhToan>()
            .Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = GetEnumDescription(e)
            }).ToList();
    }

    public static List<SelectListItem> GetSelectListTrangThaiPhong()
    {
        return Enum.GetValues(typeof(enTrangThaiPhong))
            .Cast<enTrangThaiPhong>()
            .Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = GetEnumDescription(e)
            }).ToList();
    }

    public static List<SelectListItem> GetSelectListLoaiGiuong()
    {
        return Enum.GetValues(typeof(enLoaiGiuong))
            .Cast<enLoaiGiuong>()
            .Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = GetEnumDescription(e)
            }).ToList();
    }

    public static List<SelectListItem> GetSelectListLoaiView()
    {
        return Enum.GetValues(typeof(enLoaiView))
            .Cast<enLoaiView>()
            .Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = GetEnumDescription(e)
            }).ToList();
    }


    private static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }
    public static string ToRoleName(this enRoles role)
    {
        return role.ToString().ToUpper(); // hoặc viết thường nếu cần
    }
}
