using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcCodeFlowClientManual.Models;

namespace MvcCodeFlowClientManual {
    
    public static class DefaultClientConfiguration {

        public static readonly string ClientId = Configuration.Get<string>("clientId");

        public static readonly string ClientSecret = Configuration.Get<string>("clientSecret");

        public static readonly string CallbackUrl = "https://localhost:44312/callback";

        public static readonly string Tenant = Configuration.Get<string>("tenant");

        public static readonly string ArloAuthApiGetEvents = Configuration.Get<string>("getEventsUrl");

        public static OAuthClientConfiguration LastUsed { get; set; }

        public static OAuthClientConfiguration UsableModel {
            get {
                if (LastUsed == null) {
                    LastUsed = new OAuthClientConfiguration {
                        AuthorizationServer = Constants.AuthorizeEndpoint,
                        ClientId = ClientId,
                        ClientSecret = ClientSecret,
                        CallbackUrl = CallbackUrl,
                        Tenant = Tenant,
                        Scopes = "openid profile read write roles all_claims"
                    };
                }
                return LastUsed;
            }
        }

    }
}