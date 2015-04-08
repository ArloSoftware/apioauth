using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using MvcCodeFlowClientManual.Models;

namespace MvcCodeFlowClientManual.Controllers {

    public class HomeController : Controller {
    
        public ActionResult Index() {
            Request.GetOwinContext().Authentication.SignOut("Cookies");
            return View(DefaultClientConfiguration.UsableModel);
        }

        [HttpPost]
        public ActionResult Index(OAuthClientConfiguration model) {
            
            var state = Guid.NewGuid().ToString("N");
            var nonce = Guid.NewGuid().ToString("N");

            SetTempState(state, nonce);

            DefaultClientConfiguration.LastUsed = model;

            var url = CreateCodeFlowUrl(state, nonce);

            return Redirect(url);
        }

        private string CreateCodeFlowUrl(string state, string nonce) {
            var model = DefaultClientConfiguration.LastUsed;
            Func<string, string> encode = s => HttpUtility.UrlEncode(s);
            return Constants.AuthorizeEndpoint
                    + "?response_type=code&client_id="
                    + model.ClientId + "&scope="
                    + model.Scopes.Replace(" ", "+")
                    + "&redirect_uri="
                    + encode(model.CallbackUrl) + "&state="
                    + state + "&nonce="
                    + nonce + "&login_hint="
                    + encode("tenant:" + model.Tenant);
        }

        private void SetTempState(string state, string nonce) {
            var cid = new ClaimsIdentity("TempState");
            cid.AddClaim(new Claim("state", state));
            cid.AddClaim(new Claim("nonce", nonce));

            Request.GetOwinContext().Authentication.SignIn(cid);
        }
    }
}