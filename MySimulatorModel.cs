using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FlightSimulatorApp;

namespace FlightSimulatorApp
{
    public class MySimulatorModel : ISimulatorModel
    {

        private ITelnetClient telnetClient;
        volatile bool stop;
        private static Mutex mut = new Mutex();

        public event PropertyChangedEventHandler PropertyChanged;

        //Dashboard variables with getters and setters

        private double degree;
        public double Degree
        {
            get { return this.degree; }
            set
            {
                if (this.degree != value)
                {
                    this.degree = value;
                    this.NotifyPropertyChanged("Degree");
                }

            }
        }

        private double verticalSpeed;
        public double VerticalSpeed
        {
            get { return this.verticalSpeed; }
            set
            {
                if (this.verticalSpeed != value)
                {
                    this.verticalSpeed = value;
                    this.NotifyPropertyChanged("VerticalSpeed");
                }

            }
        }

        private double groundSpeed;
        public double GroundSpeed
        {
            get { return this.groundSpeed; }
            set
            {
                if (this.groundSpeed != value)
                {
                    this.groundSpeed = value;
                    this.NotifyPropertyChanged("GroundSpeed");
                }

            }
        }

        private double airSpeed;
        public double AirSpeed
        {
            get { return this.airSpeed; }
            set
            {
                if (this.airSpeed != value)
                {
                    this.airSpeed = value;
                    this.NotifyPropertyChanged("AirSpeed");
                }

            }
        }

        private double gpsAltitude;
        public double GpsAltitude
        {
            get { return this.gpsAltitude; }
            set
            {
                if (this.gpsAltitude != value)
                {
                    this.gpsAltitude = value;
                    this.NotifyPropertyChanged("GpsAltitude");
                }

            }
        }

        private double rollDegree;
        public double RollDegree
        {
            get { return this.rollDegree; }
            set
            {
                if (this.rollDegree != value)
                {
                    this.rollDegree = value;
                    this.NotifyPropertyChanged("RollDegree");
                }

            }
        }

        private double pitchDegree;
        public double PitchDegree
        {
            get { return this.pitchDegree; }
            set
            {
                if (this.pitchDegree != value)
                {
                    this.pitchDegree = value;
                    this.NotifyPropertyChanged("PitchDegree");
                }

            }
        }

        private double altimeterAltitude;
        public double AltimeterAltitude
        {
            get { return this.altimeterAltitude; }
            set
            {
                if (this.altimeterAltitude != value)
                {
                    this.altimeterAltitude = value;
                    this.NotifyPropertyChanged("AltimeterAltitude");
                }

            }
        }

        //Coordinates with getters and setters

        public double longitude;
        public double Longitude
        {
            get { return this.longitude; }
            set
            {
                if (this.longitude != value)
                {
                    this.longitude = value;
                    this.NotifyPropertyChanged("Longitude");
                }

            }
        }

        public double latitude;
        public double Latitude
        {
            get { return this.latitude; }
            set
            {
                if (this.latitude != value)
                {
                    this.latitude = value;
                    this.NotifyPropertyChanged("Latitude");
                }

            }
        }

        //Err_
        private Visibility err_msg_not_connected;
        public Visibility Err_visiblity_Not_Connected
        {
            get { return err_msg_not_connected; }
            set
            {
                this.err_msg_not_connected = value;
                this.NotifyPropertyChanged("Err_visiblity_Not_Connected");

            }
        }

        private Visibility err_msg_error_server;
        public Visibility Err_visiblity_Error_Server
        {
            get { return err_msg_error_server; }
            set
            {
                this.err_msg_error_server = value;
                this.NotifyPropertyChanged("Err_visiblity_Error_Server");

            }
        }

        // Constructor -
        public MySimulatorModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.stop = true;
        }



        public void connect(string ip, int port)
        {
      
            
           
            telnetClient.connect(ip, port);
            Err_visiblity_Not_Connected = Visibility.Collapsed;

            stop = false;
            
            Start();

        }

        public void disconnect()
        {
            stop = true;
            Err_visiblity_Not_Connected = Visibility.Visible;
            telnetClient.disconnect();

        }

        public void Start()
        {

            //getting the dashboard information
            new Thread(delegate ()
            {
                if (Thread.CurrentThread.Name == null) 
                { 
                Thread.CurrentThread.Name = "RequestPropsThread";
                  }

                while (!stop)
                {
                    Console.WriteLine("entered new thread");
                    mut.WaitOne();
                    try
                    {
                        telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                        Degree = Math.Round(double.Parse(telnetClient.read()), 4);
                    }
                    catch (FormatException e)
                    {
                        Got_Format_Error();
                    }


                    telnetClient.write("get /instrumentation/gps/indicated-vertical-speed\n");
                    VerticalSpeed = Math.Round(double.Parse(telnetClient.read()), 4);

                    telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                    GroundSpeed = Math.Round(double.Parse(telnetClient.read()), 4);

                    telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                    AirSpeed = Math.Round(double.Parse(telnetClient.read()), 4);

                    telnetClient.write("get /instrumentation/gps/indicated-altitude-ft\n");
                    GpsAltitude = Math.Round(double.Parse(telnetClient.read()), 4);

                    telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                    RollDegree = Math.Round(double.Parse(telnetClient.read()), 4);

                    telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                    PitchDegree = Math.Round(double.Parse(telnetClient.read()), 4);

                    telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                    AltimeterAltitude = Math.Round(double.Parse(telnetClient.read()), 4);

                    for (int i = 0; i < 5; i++)
                    {
                        if (!stop)
                        {
                            telnetClient.write("get /position/longitude-deg\n");
                            Longitude = Math.Round(double.Parse(telnetClient.read()), 4);
                        }
                        if (!stop)
                        {
                            telnetClient.write("get /position/latitude-deg\n");
                            Latitude = Math.Round(double.Parse(telnetClient.read()), 4);
                        }
                    }



                    Thread.Sleep(50);
                    mut.ReleaseMutex();
                }
                
                    


                
            }).Start();



        }

        void Got_Format_Error()
        {
            Err_visiblity_Error_Server = Visibility.Visible;
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        public void changeThrottle(double throttle)
        {
            if (!stop)
            {
                if (throttle < 0) { throttle = 0; }

                else if (throttle > 1) { throttle = 1; }

                telnetClient.write($"set /controls/engines/current-engine/throttle {throttle}\n");
                try
                {
                    double simReturnedValue = double.Parse(telnetClient.read());
                }
                catch { }
                

                /// if (throttle < 0 || throttle > 1) {; }  ///error,not in range. need to implement.
            }


        }

        public void changeIP(string ip)
        {
           
           

       
        }

        public void changeRudder(double rudder)
        {
            if (!stop) { 
            if (rudder < -1) { rudder = -1; }

            else if (rudder > 1) { rudder = 1; }

            telnetClient.write($"set /controls/flight/rudder {rudder}\n");
                try
                {
                    double simReturnedValue = double.Parse(telnetClient.read());
                }
                catch (FormatException) { }
            //

            /// if (rudder < -1 || rudder > 1) {; }  ///error,not in range. need to implement.
             }
        }

        public void changeCoordinates(double elevator, double aileron)
        {
            double simReturnedValue;
            if (!stop) { 
            if (elevator < -1) { elevator = -1; }

            else if (elevator > 1) { elevator = 1; }

            telnetClient.write($"set /controls/flight/elevator {elevator}\n");
                try
                {
                    simReturnedValue = double.Parse(telnetClient.read());
                }
                catch(FormatException e)
                {
                    
                }

            /// if (elevator < -1 || elevator > 1) {; }  ///error,not in range. need to implement.
            /// 
            if (aileron < -1) { aileron = -1; }

            else if (aileron > 1) { aileron = 1; }

            telnetClient.write($"set /controls/flight/aileron {aileron}\n");
                try
                {
                    simReturnedValue = double.Parse(telnetClient.read());
                }
                catch (FormatException)
                {

                }

            /// if (aileron < -1 || aileron > 1) {; }  ///error,not in range. need to implement.
            ///
            }
        }
    }
     


}