using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin
{
    /// <summary>
    /// Use this interface to provide information about your plugin.
    /// </summary>
    public interface IAboutInfo : IPlugin
    {
        /// <summary>
        /// Name of the plugin.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Description of the plugin.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Author of the plugin.
        /// </summary>
        string Author { get; }
        /// <summary>
        /// Url of the author.
        /// </summary>
        string AuthorUrl { get; }
        /// <summary>
        /// Icon of the plugin.
        /// </summary>
        Uri Icon { get; }
        /// <summary>
        /// (Project-)Url of the plugin.
        /// </summary>
        string Url { get; }
    }
}
