using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MvcCodeFlowClientManual.Models;
using MvcCodeFlowClientManual.Extensions;
using Newtonsoft.Json;

namespace MvcCodeFlowClientManual.Controllers {

    [Authorize]
    public class AppController : Controller {

        private static Dictionary<string, Func<string, string>> TokenMorphers = new Dictionary<string, Func<string, string>> {
            { "None", t => t },
            { "Empty token", t => String.Empty },
            { "Random token", t => Guid.NewGuid().ToString() }
        };

        public ActionResult Index(string message) {
            return View(new AppContainer { Message = message });
        }

        public async Task<ActionResult> CallArloService(ServiceCall call) {

            var svc = ArloClient(call, TokenMorphers[call.Simulation]);

            ServiceCallResult result = await AttemptExecution(() => svc.Item1.SendAsync(svc.Item2));

            return View("CallArloService", result);
        }

        public async Task<ActionResult> RefreshToken() {

            var principal = User as ClaimsPrincipal;

            var refreshToken = principal.FindFirst("refresh_token");

            var info = new AppContainer { Message = refreshToken == null ? "You don't have a refresh token" : null };

            if (info.Message == null) {
                HttpContent content = new [] { "grant_type", "redirect_uri", "refresh_token" }
                                    .ToOAuthFormat(new [] { "refresh_token", 
                                                            DefaultClientConfiguration.LastUsed.CallbackUrl,
                                                            refreshToken.Value });

                HttpClient client = new HttpClient()
                    .SetBasicAuth(DefaultClientConfiguration.LastUsed.ClientId, DefaultClientConfiguration.LastUsed.ClientSecret);

                var response = await client.PostAsync(Constants.TokenEndpoint, content);
                var tokenDetails = await response.Content.ReadAsStringAsync();
                UpdateCookie(JsonConvert.DeserializeObject<TokenResponse>(tokenDetails));
            }

            return RedirectToAction("Index", info);
        }

        private Tuple<HttpClient, HttpRequestMessage> ArloClient(ServiceCall call, Func<string, string> f = null) {
            var client = new HttpClient();
            var request = new HttpRequestMessage {
                RequestUri = new Uri(call.Url),
                Method = call.Method
            };
            if (call.IncludeBearer && f != null) {
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + f(call.AccessToken));
                client.DefaultRequestHeaders.Add("Accept", "application/xml");
            }
            return Tuple.Create(client, request);
        }

        private async Task<ServiceCallResult> AttemptExecution(Func<Task<HttpResponseMessage>> f) {
            ServiceCallResult ret = new ServiceCallResult();
            try {
                HttpResponseMessage response = await f();
                if (!response.IsSuccessStatusCode)
                    ret.FailureCode = "Request failed - " + response.StatusCode.ToString();
                ret.Headers =
                        String.Join("<br/>", response.Headers.Select(kvp => "<strong>" + kvp.Key + "</strong> "
                        + string.Join(", ", kvp.Value))) + "<p></p>";
                ret.Body = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex) {
                ret.FailureCode = "Unexpected exception";
                ret.Body = ex.ToString();
            }
            return ret;
        }

        private void UpdateCookie(TokenResponse response) {

            var identity = (User as ClaimsPrincipal).Identities.First();
            var result = from c in identity.Claims
                         where c.Type != "access_token" &&
                               c.Type != "refresh_token" &&
                               c.Type != "expires_at"
                         select c;

            var claims = result.ToList();

            claims.AddRange(response.Claims);
            var newId = new ClaimsIdentity(claims, "Cookies");
            Request.GetOwinContext().Authentication.SignIn(newId);
        }

    }

    public class AppContainer {

        public string Message { get; set; }

        public IEnumerable<KeyValuePair<string, string>> GroupedClaims {
            get {
                return ClaimsPrincipal.Current.Claims
                        .GroupBy(c => c.Type)
                        .Select(g => new KeyValuePair<string, string>(g.Key, String.Join(", ", g.AsEnumerable().Select(c => c.Value))))
                        .ToArray();
            }
        }

    }

}