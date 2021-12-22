using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS_Sim.Common
{
    public class SimVarTimer
    {
        public int RequestID { get; set; }
        public int TimerID { get; internal set; }
        public SimConnectVariable Request { get; set; }
        public int FrequencyInMs { get; set; }
    }
}
