﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS_Sim.Common
{
    public class SimVarMessage
    {
        public EventLogEntryType Severity { get; set; } = EventLogEntryType.Information;
        public string Message { get; set; }
    }
}
