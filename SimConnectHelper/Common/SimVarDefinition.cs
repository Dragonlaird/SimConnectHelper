using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimConnectHelper.Common
{
    internal class SimVarDefinition
    {
        internal SimVarDefinition(string name, string description, string defaultUnit, Type unitType, bool readOnly, bool multiPlayer)
        {
            Name = name;
            Description = description;
            DefaultUnit = defaultUnit;
            UnitType = unitType;
            ReadOnly = readOnly;
            MultiPlayer = multiPlayer;
        }
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal string DefaultUnit { get; set; }
        internal Type UnitType { get; set; }
        internal bool ReadOnly { get; set; }
        internal bool MultiPlayer { get; set; }
    }
}
