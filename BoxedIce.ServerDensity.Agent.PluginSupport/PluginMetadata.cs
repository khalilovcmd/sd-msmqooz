using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using log4net;
using Newtonsoft.Json;

namespace BoxedIce.ServerDensity.Agent.PluginSupport
{
    public class PluginMetadata
    {
        public PluginMetadata(string agentKey, string installKey)
        {
            _agentKey = agentKey;
            _installKey = installKey;
        }

        private string Get()
        {
            var client = new WebClient();
            var data = new NameValueCollection();
            data.Add("agentKey", _agentKey);
            data.Add("installId", _installKey);
            var url = string.Format("{0}install/", BASE_URL);
            Log.InfoFormat("Posting to {0}", url);

            if (HttpWebRequest.DefaultWebProxy != null)
            {
                client.Proxy = HttpWebRequest.DefaultWebProxy;
            }

            byte[] response = client.UploadValues(url, "POST", data);
            string responseText = Encoding.ASCII.GetString(response);
            client.Dispose();
            return responseText;
        }

        public IDictionary<string, object> Json()
        {
            IDictionary<string, object> results = JsonConvert.DeserializeObject<IDictionary<string, object>>(Get());
            if (results.ContainsKey("status") && results["status"].ToString() == "error")
            {
                throw new Exception(results["msg"].ToString());
            }
            return results;
        }

        private string _agentKey;
        private string _installKey;
        private readonly static ILog Log = LogManager.GetLogger(typeof(PluginMetadata));
        private const string BASE_URL = "http://plugins.serverdensity.com/";
    }
}
