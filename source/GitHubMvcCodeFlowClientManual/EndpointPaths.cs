using System;
using MvcCodeFlowClientManual.Models;

namespace MvcCodeFlowClientManual
{
    public static class EndpointPaths
    {
        public static Uri GetAuthorizeEndpointUri(OAuthClientConfiguration config) {
            return new Uri(new Uri(config.AuthorizationServerRootUri), "./connect/authorize");
        }

        public static Uri GetLogoutEndpointUri(OAuthClientConfiguration config) {
            return new Uri(new Uri(config.AuthorizationServerRootUri), "./connect/endsession");
        }

        public static Uri GetTokenEndpointUri(OAuthClientConfiguration config) {
            return new Uri(new Uri(config.AuthorizationServerRootUri), "./connect/token");
        }

        public static Uri GetUserInfoEndpointUri(OAuthClientConfiguration config) {
            return new Uri(new Uri(config.AuthorizationServerRootUri), "./connect/userinfo");
        }

        public static Uri GetIdentityTokenValidationEndpointUri(OAuthClientConfiguration config) {
            return new Uri(new Uri(config.AuthorizationServerRootUri), "./connect/identitytokenvalidation");
        }

        public static Uri GetPermissionsEndpointUri(OAuthClientConfiguration config) {
            return new Uri(new Uri(config.AuthorizationServerRootUri), "./permissions");
        }
    }
}