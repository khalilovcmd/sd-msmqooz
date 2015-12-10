using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BoxedIce.ServerDensity.Agent.PluginSupport
{
    public class PluginInstallManager
    {
        public PluginInstallManager(string agentKey, string installKey, string pluginPath)
        {
            _agentKey = agentKey;
            _installKey = installKey;
            _pluginPath = pluginPath;
        }

        public void Start()
        {
            _thread = new Thread(new ThreadStart(Run));
            _thread.Name = "Server Density Agent Plugin Installer";
            _thread.Start();
        }

        private void Run()
        {
            try
            {
                IDictionary<string, object> metadata = new PluginMetadata(_agentKey, _installKey).Json();
                OnMetadataComplete(EventArgs.Empty);
                PluginDownloader downloader = new PluginDownloader(_agentKey, _installKey, _pluginPath, metadata);
                Thread.Sleep(1000);
                downloader.Start();
                OnDownloadComplete(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                OnError(new ErrorEventArgs(ex));
            }
        }

        private void OnMetadataComplete(EventArgs e)
        {
            if (MetadataComplete == null)
            {
                return;
            }
            MetadataComplete(this, e);
        }

        private void OnDownloadComplete(EventArgs e)
        {
            if (DownloadComplete == null)
            {
                return;
            }
            DownloadComplete(this, e);
        }

        private void OnError(ErrorEventArgs e)
        {
            if (Error == null)
            {
                return;
            }
            Error(this, e);
        }

        private readonly string _agentKey;
        private readonly string _installKey;
        private readonly string _pluginPath;
        private Thread _thread;

        public event EventHandler MetadataComplete;
        public event EventHandler DownloadComplete;
        public event ErrorEventHandler Error;
    }
}
