using QuickNavPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNav.Helper
{
    public static class QueryHelper
    {
        public static string FixQuery(ICommand cmd, string query)
        {
            query = query.Trim();
            if(cmd.CommandTrigger != null && query.ToLower().StartsWith(cmd.CommandTrigger.ToLower()))
                return query.Substring(cmd.CommandTrigger.Length).Trim();
            return query;
        }
    }
}
