# SimConnectHelper

A static C# class to simplify communication with MSFS 2020 using the SimConnect SDK (aka SimConnect).

Allows a project to connect to MSFS 2020, via SimConnect, and submit requests for SimVars.

MSFS 2020 may be running on the local PC, or on a remote PC.

Can connect via TCP/IP (v4, v6 or Named Pipes), if running on a remote PC, if the remote PC firewall is configured to allow remote connections and the SimConnect.xml file is configured to accept them.

Each SimVar request can be set to auto-update using preset SimConnect periods or specify you own custom frequency in milliseconds

SimVars can be updated, but are likely to overwritten by MSFS 2020's own AI updates, adding code to allow this auto-update to be disabled in a new branch soon.
