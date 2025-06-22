using System.Collections.Generic;

namespace DPKS.Admin.Models
{
    public class ModalFormRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string FormId { get; set; }
        public string UrlSubmit { get; set; }
        /// <summary>
        /// url path để load data sau khi submit
        /// </summary>
        public string UrlPathLoadData { get; set; }
        public Dictionary<string, object> QueryLoadData { get; set; }
        /// <summary>
        /// Id của element muốn load lại data
        /// </summary>
        public string LoadDataId { get; set; }
        public bool AutoCloseModal { get; set; } = false;
        public bool IsReload { get; set; }
        public string RedirectUrl { get; set; }

        public string FunctionCallBack { get; set; }
    }
}
