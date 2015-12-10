using System;
using System.Collections.Generic;
using System.Text;

namespace BoxedIce.ServerDensity.Agent.PluginSupport
{
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

    public class ErrorEventArgs : EventArgs
    {
        public Exception Exception
        {
            get { return _exception; }
        }

        public ErrorEventArgs(Exception exception) : base()
        {
            _exception = exception;
        }

        private Exception _exception;
    }
}
