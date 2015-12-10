using System;
using System.Collections.Generic;
using System.Text;

namespace BoxedIce.ServerDensity.Agent.PluginSupport
{
    /// <summary>
    /// Interface that must be implemented by any check class.
    /// </summary>
    public interface ICheck
    {
        /// <summary>
        /// Gets the key name for the check.
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Performs the check.
        /// </summary>
        object DoCheck();
    }
}
