using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace MvcCodeFlowClientManual.Models {

    public class TokenResponse {
        public string raw { get; set; }
        public string id_token { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }
        public bool is_error { get; set; }
        public IEnumerable<Claim> Claims { 
            get {  
                var claims = new List<Claim>();
                if (!string.IsNullOrWhiteSpace(id_token)) {
                    if (!string.IsNullOrWhiteSpace(access_token)) {
                        claims.Add(new Claim("access_token", access_token));
                        claims.Add(new Claim("expires_at", (DateTime.UtcNow.AddSeconds(expires_in)).ToLocalTime().ToString()));
                    }
                    if (!string.IsNullOrWhiteSpace(refresh_token)) {
                        claims.Add(new Claim("refresh_token", refresh_token));
                    }
                }
                return claims;
            } 
        }
    }
}