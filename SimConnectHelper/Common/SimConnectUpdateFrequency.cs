using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS_Sim.Common
{
    public enum SimConnectUpdateFrequency // SIMCONNECT_PERIOD
    {
        Never = 0,
        Once = 1,
        Visual_Frame = 2,
        SIM_Frame = 3,
        Second = 4
    }
}
