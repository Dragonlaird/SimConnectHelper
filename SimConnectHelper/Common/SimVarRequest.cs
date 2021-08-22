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
        private string simVarName { get
            {
                if (this.Request == null)
                    return null;
                var simVarName = this.Request.Name;
                if (!string.IsNullOrEmpty(simVarName) && simVarName.IndexOf(':') > -1)
                    simVarName = simVarName.Substring(0, simVarName.IndexOf(':'));
                return simVarName;
            } 
        }

        internal int ID { get; set; }

        internal SimConnectVariable Request { get; set; }

        internal Type DataType => SimVarUnits.DefaultUnits.Any(x => x.Key == simVarName) ?
            SimVarUnits.DefaultUnits[simVarName].UnitType : 
            typeof(string);

        internal SIMCONNECT_DATATYPE SimType
        {
            get
            {
                return SimVarUnits.GetSimVarType(this.DataType?.ToString() ?? SimVarUnits.DefaultUnits[simVarName].DefaultUnit);
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
