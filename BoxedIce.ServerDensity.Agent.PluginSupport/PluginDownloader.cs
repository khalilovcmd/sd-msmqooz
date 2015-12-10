using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using log4net;

namespace BoxedIce.ServerDensity.Agent.PluginSupport
{
    public class PluginDownloader
    {
        public PluginDownloader(string agentKey, string installKey, string pluginPath, IDictionary<string, object> metadata)
        {
            _agentKey = agentKey;
            _installKey = installKey;
            _pluginPath = pluginPath;
            _metadata = metadata;
        }

        public void Start()
        {
            var client = new WebClient();
            var url = string.Format("{0}download/{1}/{2}", BASE_URL, _installKey, _agentKey);
            Log.InfoFormat("Posting to {0}", url);

            if (HttpWebRequest.DefaultWebProxy != null)
            {
                client.Proxy = HttpWebRequest.DefaultWebProxy;
            }

            byte[] response = client.DownloadData(url);
            string path = Path.Combine(_pluginPath, string.Format("{0}.zip", _metadata["name"]));
            using (FileStream outStream = File.OpenWrite(path))
            {
                outStream.Write(response, 0, response.Length);
            }

            client.Dispose();

            using (ZipFile file = new ZipFile(path))
            {
                foreach (ZipEntry entry in file)
                {
                    string outPath = Path.Combine(_pluginPath, entry.Name);
                    using (Stream inStream = file.GetInputStream(entry))
                    {
                        using (FileStream outStream = File.OpenWrite(outPath))
                        {
                            byte[] buffer = new byte[entry.Size];
                            inStream.Read(buffer, 0, buffer.Length);
                            outStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }

            File.Delete(path);
        }

        private readonly string _agentKey;
        private readonly string _installKey;
        private readonly string _pluginPath;
        IDictionary<string, object> _metadata;
        private static readonly ILog Log = LogManager.GetLogger(typeof(PluginDownloader));
        private const string BASE_URL = "http://plugins.serverdensity.com/";
    }
}
