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

    public static string ToRoleName(this enRoles role)
    {
        return role.ToString().ToUpper(); // hoặc viết thường nếu cần
    }
}
