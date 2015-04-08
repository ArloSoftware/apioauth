using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;

namespace MvcCodeFlowClientManual.Models {

    public class ServiceCall {

        public ServiceCall() {
            Method = HttpMethod.Get;
            IncludeBearer = true;
        }

        [Display(Name = "Endpoint URL")]
        public string Url { get; set; }
        
        [Display(Name = "Simulation required")]
        public string Simulation { get; set; }
        
        public string AccessToken { get; set; }

        [ScaffoldColumn(true)]
        public HttpMethod Method { get; set; }

        [ScaffoldColumn(true)]
        public bool IncludeBearer { get; set; } 
    }

}