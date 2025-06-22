using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.Admin.Models
{
    public class FormRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string FromId { get; set; }
        public string Url { get; set; }
        public string UrlBack { get; set; }
        public string CallFuntion { get; set; }
        public bool IsLoadPage { get; set; } = true;
        public bool IsMessage { get; set; } = true;
    }
}
