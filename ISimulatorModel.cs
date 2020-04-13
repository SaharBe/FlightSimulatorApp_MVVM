using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace FlightSimulatorApp
{
    public interface ISimulatorModel : INotifyPropertyChanged
    {

        void connect(string ip, int port);

        void Start();
        void disconnect();

        double Degree
        {
            get;
            set;
        }

        double VerticalSpeed
        {
            get;
            set;
        }

        double GroundSpeed
        {
            get;
            set;
        }

        double AirSpeed
        {
            get;
            set;
        }

        double GpsAltitude
        {
            get;
            set;
        }

        double RollDegree
        {
            get;
            set;
        }

        double PitchDegree
        {
            get;
            set;
        }

        double AltimeterAltitude
        {
            get;
            set;
        }

        double Longitude
        {
            get;
            set;
        }

        double Latitude
        {
            get;
            set;
        }
        Visibility Err_Out_Of_Bounds { get; set; }
        Visibility Err_Server_IO { get; set; }

        Visibility Err_Server_Format { get; set; }
        Visibility Err_visiblity_Not_Connected { get; set; }

        void changeThrottle(double value);
        void changeRudder(double value);
        void changeCoordinates(double elevator, double aileron);

     
    }
}