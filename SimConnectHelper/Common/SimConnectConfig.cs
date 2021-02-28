using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SimConnectHelper.Common
{
    [XmlRoot("SimConnect.Comm")]
    public class SimConnectConfig
    {
        /*
        <SimConnect.Comm>
            <Descr>Static IP4 port</Descr>
            <Protocol>IPv4</Protocol>
            <Scope>local</Scope>
            <Port>500</Port>
            <MaxClients>64</MaxClients>
            <MaxRecvSize>41088</MaxRecvSize>
        </SimConnect.Comm>
        */
        [XmlElement("Descr")]
        public string Descr { get; set; } = "New Configuration";
        [XmlElement("Protocol")]
        public SimConnectConfigProtocol Protocol { get; set; } = SimConnectConfigProtocol.IPv4;
        [XmlElement("Address")]
        public string Address { get; set; }
        [XmlElement("Scope")]
        public SimConnectConfigScope Scope { get; set; } = SimConnectConfigScope.local;
        [XmlElement("Port")]
        public string Port { get; set; } = "500";
        [XmlElement("MaxClients")]
        public int MaxClients { get; set; } = 64;
        [XmlElement("MaxRecvSize")]
        public int MaxRecvSize { get; set; } = 41088;
        [XmlElement("DisableNagle")]
        public bool DisableNagle { get; set; } = true;

        public string ConfigFileText
        {
            get
            {
                const string configTemplate = @"[simConnect]
Protocol={0}
Address={1}
Port={2}
MaxReceiveSize={3}
DisableNagle={4}";
                var address = Address;
                if(string.IsNullOrEmpty(address))
                    switch (Protocol)
                    {
                        case SimConnectConfigProtocol.IPv6:
                            address = IPAddress.IPv6Loopback.ToString();
                            break;
                        default:
                            address = IPAddress.Loopback.ToString();
                            break;
                    }
                return string.Format(configTemplate, Protocol.ToString(), address, Port, MaxRecvSize, DisableNagle ? 1 : 0);
            }
        }
    }
}
