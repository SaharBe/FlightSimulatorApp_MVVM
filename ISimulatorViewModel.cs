using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace FlightSimulatorApp
{
    public interface ISimulatorViewModel : INotifyPropertyChanged
    {
        void connect(object o);
        void disconnect();
    }
}