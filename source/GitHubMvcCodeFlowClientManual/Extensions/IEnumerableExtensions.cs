using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace MvcCodeFlowClientManual.Extensions {
    
    public static class IEnumerableExtensions {

        public static FormUrlEncodedContent ToOAuthFormat(this IEnumerable<string> keys, IEnumerable<string> values) {
            Func<string, string, KeyValuePair<string, string>> formField = (k, v) => new KeyValuePair<string, string>(k, v);
            return new FormUrlEncodedContent(keys.Zip(values, (k, v) => formField(k, v)).ToArray());
        }

    }
}