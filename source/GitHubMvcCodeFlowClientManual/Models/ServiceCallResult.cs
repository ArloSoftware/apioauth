using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcCodeFlowClientManual.Models {

    public class ServiceCallResult {
        public string Headers { get; set; }
        public string Body { get; set; }
        public string FailureCode { get; set; }
    }

}