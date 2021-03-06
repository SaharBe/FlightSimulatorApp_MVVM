﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using FlightSimulatorAppv;
using System.Linq.Expressions;

namespace FlightSimulatorApp
{
    public class MyTelnetClient : ITelnetClient
    {
        TcpClient tcpClient;
        NetworkStream netStream;
        

        public void connect(string ip, int port)
        {
            try
            {
                tcpClient = new TcpClient(ip, port);
                netStream = tcpClient.GetStream();
                netStream.ReadTimeout = 10000;
            }
            catch (IOException e)
            {
                throw new IOException();

            }

        }

        public void disconnect()
        {
            if (isConnected()){
                netStream.Close();
                tcpClient.Close();
            }
        }

        public string read()
        {
            if (isConnected())
            {
                byte[] myReadBuffer = new byte[256];
                try
                {
                    netStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                }
                catch (IOException e)
                {
                    Console.WriteLine("server isn't sending output.disconnecting.");
                    throw new IOException();
                    
                    
                  
                    

                }
                catch(FormatException e)
                {
                    Console.WriteLine("Error in format sent from server");
                    throw new FormatException();
                }

                string commandRecived = Encoding.ASCII.GetString(myReadBuffer);
                
                return commandRecived;
            }
            return null;
            
        }

        public void write(string command)
        {
            if (isConnected())
            {
                byte[] commandToSend = Encoding.ASCII.GetBytes(command);
                try
                {
                    netStream.Write(commandToSend, 0, commandToSend.Length);
                }
                catch (IOException e)
                {
                    Console.WriteLine("Error in writing to server");
                }
            }
        }

        public bool isConnected()
        {
           
            if(tcpClient == null)
            {
                return false;
            }
           
            return tcpClient.Connected;
            
 
            
        }
    }
}

