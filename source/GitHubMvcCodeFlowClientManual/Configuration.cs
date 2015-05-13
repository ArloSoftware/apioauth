using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace MvcCodeFlowClientManual {

    public static class Configuration {

        private const string StandardPrefix = "arlo:";

        public static T Get<T>(string key, T defaultValue = default(T)) {
            var setting = ConfigurationManager.AppSettings[StandardPrefix + key];
            return setting == null ? defaultValue : (T)Convert.ChangeType(setting, typeof(T));
        }

    }

}