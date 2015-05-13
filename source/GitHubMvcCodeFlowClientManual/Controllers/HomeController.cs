using System;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MvcCodeFlowClientManual.Models;

namespace MvcCodeFlowClientManual.Controllers {

    public class HomeController : Controller {
    
        public ActionResult Index() {
            Request.GetOwinContext().Authentication.SignOut("Cookies");
            return View(DefaultClientConfiguration.CurrentOAuthConfig);
        }

        [HttpPost]
        public ActionResult Index(OAuthClientConfiguration model) {
            
            var state = Guid.NewGuid().ToString("N");
            var nonce = Guid.NewGuid().ToString("N");

            SetTempState(state, nonce);

            DefaultClientConfiguration.CurrentOAuthConfig = model;

            var url = CreateCodeFlowUrl(state, nonce);

            return Redirect(url);
        }

        private string CreateCodeFlowUrl(string state, string nonce) {
            var model = DefaultClientConfiguration.CurrentOAuthConfig;

            var requestParams = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("response_type", "code"),
                new KeyValuePair<string, string>("client_id", model.ClientId),
                new KeyValuePair<string, string>("scope", model.Scopes),
                new KeyValuePair<string, string>("redirect_uri", model.CallbackUrl),
                new KeyValuePair<string, string>("state", state),
                new KeyValuePair<string, string>("nonce", nonce),
                new KeyValuePair<string, string>("login_hint", "tenant:" + model.Tenant),
            };

            string queryString = string.Join("&", requestParams.Select(p => string.Format("{0}={1}", Uri.EscapeDataString(p.Key), Uri.EscapeDataString(p.Value ?? string.Empty))).ToArray());

            return string.Format("{0}?{1}", EndpointPaths.GetAuthorizeEndpointUri(model), queryString);/*
                    + "?response_type=code&client_id="
                    + model.ClientId + "&scope="
                    + model.Scopes.Replace(" ", "+")
                    + "&redirect_uri="
                    + encode(model.CallbackUrl) + "&state="
                    + state + "&nonce="
                    + nonce + "&login_hint="
                    + encode("tenant:" + model.Tenant);*/
        }

        private void SetTempState(string state, string nonce) {
            var cid = new ClaimsIdentity("TempState");
            cid.AddClaim(new Claim("state", state));
            cid.AddClaim(new Claim("nonce", nonce));

            Request.GetOwinContext().Authentication.SignIn(cid);
        }
    }
}