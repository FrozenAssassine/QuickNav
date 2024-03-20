using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin
{
    /// <summary>
    /// Use this interface for commands that need to be aborted when they are no longer in use.
    /// </summary>
    public interface IAbort : IPlugin
    {
        /// <summary>
        /// This function is called to abort the command.
        /// </summary>
        void Abort();
    }
}
