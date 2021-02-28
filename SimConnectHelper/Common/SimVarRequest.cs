using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimConnectHelper.Common
{
    [System.Diagnostics.DebuggerDisplay("\\{SimVarRequest\\} {Name}")]
    internal class SimVarRequest
    {
        internal int ID { get; set; }

        internal SimConnectVariable Request { get; set; }
        internal Type Type => SimVarUnits.DefaultUnits[this.Request.Name].UnitType;
        internal SIMCONNECT_DATATYPE SimType
        {
            get
            {
                return SimVarUnits.GetSimVarType(this.Type?.ToString() ?? SimVarUnits.DefaultUnits[this.Request.Name].DefaultUnit);
            }
        }

        internal SIMVARREQUEST ReqID
        {
            get
            {
                return (SIMVARREQUEST)this.ID;
            }
        }
        internal SIMVARDEFINITION DefID
        {
            get
            {
                return (SIMVARDEFINITION)this.ID;
            }
        }
    }
}
