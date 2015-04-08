using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using MvcCodeFlowClientManual.Models;
using MvcCodeFlowClientManual.Extensions;

namespace MvcCodeFlowClientManual.Controllers {
    
    public class CallbackController : Controller {

        public async Task<ActionResult> Index() {
            ViewBag.Code = Request.QueryString["code"] ?? "none";

            var state = Request.QueryString["state"];
            var tempState = await GetTempStateAsync();
            var desc = state.Equals(tempState.Item1, StringComparison.Ordinal) ? "valid" : "invalid";
            ViewBag.State = state + " (" + desc + ")";
            ViewBag.Error = Request.QueryString["error"] ?? "none";

            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<ActionResult> GetToken() {
            var code = Request.QueryString["code"];
            var tempState = await GetTempStateAsync();

            Request.GetOwinContext().Authentication.SignOut("TempState");

            HttpContent content = new [] { "code", "grant_type", "redirect_uri"}
                                    .ToOAuthFormat(new [] { code, "authorization_code", DefaultClientConfiguration.LastUsed.CallbackUrl });

            HttpClient client = new HttpClient()
                .SetBasicAuth(DefaultClientConfiguration.LastUsed.ClientId, DefaultClientConfiguration.LastUsed.ClientSecret);

            var response = await client.PostAsync(Constants.TokenEndpoint, content);
            var tokenDetails = await response.Content.ReadAsStringAsync();

            var decoded = JsonConvert.DeserializeObject<TokenResponse>(tokenDetails);
            decoded.raw = tokenDetails;

            await ValidateResponseAndSignInAsync(decoded, tempState.Item2);

            return View("Token", decoded);
        }

        private async Task ValidateResponseAndSignInAsync(TokenResponse response, string nonce) {

            if (!string.IsNullOrWhiteSpace(response.id_token)) {
                var id = new ClaimsIdentity(response.Claims, "Cookies");
                Request.GetOwinContext().Authentication.SignIn(id);
            }
        }

        private async Task<Tuple<string, string>> GetTempStateAsync() {
            var data = await Request.GetOwinContext().Authentication.AuthenticateAsync("TempState");

            var state = data.Identity.FindFirst("state").Value;
            var nonce = data.Identity.FindFirst("nonce").Value;

            return Tuple.Create(state, nonce);
        }
    }
    
}