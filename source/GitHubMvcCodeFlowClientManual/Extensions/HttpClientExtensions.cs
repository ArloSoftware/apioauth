using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Text;

namespace MvcCodeFlowClientManual.Extensions {
    
    public static class HttpClientExtensions {

        public static HttpClient SetBasicAuth(this HttpClient client, string userName, string password) {
            var byteArray = Encoding.ASCII.GetBytes(userName + ":" + password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return client;
        }

    }
}