using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DPKS.Common.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using DPKS.Common.System;

namespace DPKS.Common
{
    public static class SystemAlerts
    {
        public static Result<string> GetAlert(this ITempDataDictionary tempData)
        {
            CreateAlertTempData(tempData);
            return DeserializeAlerts(tempData[SystemConstants.Alerts] as string);
        }

        public static void CreateAlertTempData(this ITempDataDictionary tempData)
        {
            if (!tempData.ContainsKey(SystemConstants.Alerts))
            {
                tempData[SystemConstants.Alerts] = null;
            }

        }
        public static void AddAlert(this ITempDataDictionary tempData, Result<string> alert)
        {
            if (alert == null)
            {
                throw new ArgumentNullException(nameof(alert));
            }
            tempData[SystemConstants.Alerts] = SerializeAlerts(alert);
        }
        public static string SerializeAlerts(Result<string> tempData)
        {
            return JsonConvert.SerializeObject(tempData);
        }
        public static Result<string> DeserializeAlerts(string tempData)
        {
            if (tempData != null)
            {
                if (tempData.Length == 0)
                {
                    return Result<string>.Success();
                }
                return JsonConvert.DeserializeObject<Result<string>>(tempData);
            }
            else
            {
                return null;
            }
        }
    }
}
