﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Interfaces
{
    public interface ITool
    {
        ILogger? Logger { get; }
    }
}
