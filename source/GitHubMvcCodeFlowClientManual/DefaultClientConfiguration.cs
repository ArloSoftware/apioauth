using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcCodeFlowClientManual.Models;

namespace MvcCodeFlowClientManual {
    
    public static class DefaultClientConfiguration {

        private static OAuthClientConfiguration currentInstance;

        public static Uri GetArloAuthApiTestResourceUri(OAuthClientConfiguration config) {
            string apiHost = new Uri(config.AuthorizationServerRootUri).DnsSafeHost;
            return new Uri(string.Format("https://{0}/{1}/api/2012-02-01/auth/resources/events/", apiHost, config.Tenant));
        }

        private static Uri GetHttpApplicationBaseUri(HttpContext context) {
            return new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath);
        }

        private static OAuthClientConfiguration CreateDefaultConfig() {
            return new OAuthClientConfiguration {
                AuthorizationServerRootUri = VirtualPathUtility.AppendTrailingSlash(Configuration.Get<string>("defaultAuthServerRootUri")),
                ClientId = Configuration.Get<string>("defaultClientId"),
                ClientSecret = Configuration.Get<string>("defaultClientSecret"),
                CallbackUrl = new Uri(GetHttpApplicationBaseUri(HttpContext.Current), "./callback").AbsoluteUri,
                Tenant = Configuration.Get<string>("defaultTenant"),
                Scopes = "openid profile read write roles all_claims"
            };
        }

        public static OAuthClientConfiguration CurrentOAuthConfig {
            get {
                if (currentInstance == null) {
                    currentInstance = CreateDefaultConfig();
                }
                return currentInstance;
            }
            set {
                currentInstance = value;
            }
        }
    }
}