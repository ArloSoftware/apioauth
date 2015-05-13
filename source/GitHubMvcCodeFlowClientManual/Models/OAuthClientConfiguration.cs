using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcCodeFlowClientManual.Models {
    
    public class OAuthClientConfiguration {

        [Required]
        [Display(Name = "Target oauth server root URI")]
        public string AuthorizationServerRootUri { get; set; }

        [Required]
        [Display(Name = "Client Id (APIKey)")]
        public string ClientId { get; set; }

        [Required]
        [Display(Name = "Client secret")]
        public string ClientSecret { get; set; }

        [Required]
        [Display(Name = "Callback Url")]
        public string CallbackUrl { get; set; }

        [Required]
        [Display(Name = "Tenant")]
        public string Tenant { get; set; }

        [Required]
        [Display(Name = "Scopes")]
        public string Scopes { get; set; }

    }
}