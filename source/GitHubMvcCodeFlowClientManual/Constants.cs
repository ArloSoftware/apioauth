namespace MvcCodeFlowClientManual
{
    public static class Constants
    {
        public static readonly string BaseAddress = Configuration.Get<string>("authServer");

        public static readonly string AuthorizeEndpoint = BaseAddress + "/connect/authorize";
        public static readonly string LogoutEndpoint = BaseAddress + "/connect/endsession";
        public static readonly string TokenEndpoint = BaseAddress + "/connect/token";
        public static readonly string UserInfoEndpoint = BaseAddress + "/connect/userinfo";
        public static readonly string IdentityTokenValidationEndpoint = BaseAddress + "/connect/identitytokenvalidation";
        public static readonly string PermissionsEndpoint = BaseAddress + "/permissions";

    }
}