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
            if(cmd is ITriggerCommand trigger && query.ToLower().StartsWith(trigger.CommandTrigger.ToLower()))
                return query.Substring(trigger.CommandTrigger.Length).Trim();
            return query;
        }
    }
}
