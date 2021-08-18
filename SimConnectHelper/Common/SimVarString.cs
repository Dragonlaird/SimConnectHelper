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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct SimVarString
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string Value;
    }
}
