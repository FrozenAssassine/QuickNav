﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavPlugin
{
    public interface IBackgroundService : IPlugin
    {
        int GetIntervallMin();
        void Elapsed();
    }
}
