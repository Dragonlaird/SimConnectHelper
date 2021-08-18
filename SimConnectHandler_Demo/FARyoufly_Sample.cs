using SimConnectHelper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimConnectHandler_Demo
{
    public class FARyoufly_Sample
    {
		private IDictionary<string, SimConnectVariableValue> latestResults = new Dictionary<string, SimConnectVariableValue>();

		public void Main()
		{
			SimConnectHelper.SimConnectHelper.SimError += SimConnect_Error;
			SimConnectHelper.SimConnectHelper.SimConnected += SimConnect_Connection;
			SimConnectHelper.SimConnectHelper.SimData += SimConnect_DataReceived;

			SimConnectHelper.SimConnectHelper.Connect();

			var simVarName = "GPS GROUND TRUE HEADING"; // Or "HEADING INDICATOR", "PLANE HEADING DEGREES GYRO", "INDUCTOR COMPASS HEADING REF", "PLANE HEADING DEGREES MAGNETIC", "PLANE HEADING DEGREES TRUE"

			var variable = new SimConnectVariable
			{
				Name = simVarName,
				Unit = "radians"
			};
			var requestID = SimConnectHelper.SimConnectHelper.GetSimVar(variable, 50); // Ask SimConnect to send the value every 50ms

			for (var i = 0; i < 100; i++)
			{
				// Latest value of the SimVar can be retrieved by calling method below
				var planeHeading = GetSimVarValue(simVarName);
				// If value hasn't been returned yet, it will default to zero
				Console.WriteLine($"{i:000}: Heading: {planeHeading}");
				Thread.Sleep(10); // Wait for 10ms before checking again
			}
		}

		private double GetSimVarValue(string simVarName)
        {
			if (latestResults.ContainsKey(simVarName))
				return (double)latestResults[simVarName].Value;
			return 0;
        }

		private void SimConnect_DataReceived(object sender, SimConnectVariableValue e)
		{
			lock (latestResults)
			{
				if (latestResults.ContainsKey(e.Request.Name))
					latestResults[e.Request.Name] = e;
				else
					latestResults.Add(e.Request.Name, e);
			}
		}

		private void SimConnect_Error(object sender, ExternalException e)
		{
			throw new NotImplementedException();
		}

		private void SimConnect_Connection(object sender, bool e)
		{
            if (e)
            {
				// SimConnect has connected successfully
            }
            else
            {
				// SimConnect has disconnected
			}
		}
	}
}
