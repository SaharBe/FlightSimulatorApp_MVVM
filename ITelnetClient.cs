using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp
{
    public interface ITelnetClient
    {
        void connect(string ip, int port);
        void disconnect();
        void write(string command);
        string read();

        bool isConnected();


    }
}