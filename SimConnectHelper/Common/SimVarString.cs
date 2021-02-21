using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimConnectHelper.Common
{

    [DebuggerDisplay("\\{SimVarString\\} {Value}")]
    internal struct SimVarString
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Value;
    }
}
