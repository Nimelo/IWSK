using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Core
{
    public class SerialPortWrapper
    {
        #region Fields & Properties

        private SerialPort _port;

        private string temporaryString;
        #endregion

        #region Ctors & DCtors

        public SerialPortWrapper()
        {
            this._port = new SerialPort();
            _port.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
            this.StopCharacters = "\n";
            this.temporaryString = string.Empty;
        }


        ~SerialPortWrapper()
        {
            _port.DataReceived -= _serialPort_DataReceived;
            if(this._port.IsOpen)
                this._port.Close();

        }
        #endregion

        #region Configuration Properties
        public string Name
        {
            get
            {
                return this._port.PortName;
            }
            set
            {
                this._port.PortName = value;
            }

        }

        public int BaudRate
        {
            get
            {
                return this._port.BaudRate;
            }
            set
            {
                this._port.BaudRate = value;
            }
        }

        public Parity Parity
        {
            get
            {
                return this._port.Parity;
            }
            set
            {
                this._port.Parity = value;
            }
        }

        public int DataBits
        {
            get
            {
                return this._port.DataBits;
            }
            set
            {
                this._port.DataBits = value;
            }
        }

        public StopBits StopBits
        {
            get
            {
                return this._port.StopBits;
            }
            set
            {
                this._port.StopBits = value;
            }
        }

        public Handshake Handshake
        {
            get
            {
                return this._port.Handshake;
            }
            set
            {
                this._port.Handshake = value;
            }
        }

        public int ReadTimeout
        {
            get
            {
                return this._port.ReadTimeout;
            }
            set
            {
                this._port.ReadTimeout = value;
            }
        }

        public int WriteTimeout
        {
            get
            {
                return this._port.WriteTimeout;
            }
            set
            {
                this._port.WriteTimeout = value;
            }
        }

        private string _stopCharacters;
        public string StopCharacters
        {
            get
            {
                return this._stopCharacters;
            }
            set
            {
                this._stopCharacters = value;
            }
        }
        #endregion

        #region Lines Properties

        public bool DTR
        {
            get
            {
                return this._port.DtrEnable;
            }
            set
            {
                this._port.DtrEnable = value;
            }
        }

        public bool DSR
        {
            get
            {
                return this._port.DsrHolding;
            }
        }

        public bool RTS
        {
            get
            {
                return this._port.RtsEnable;
            }
            set
            {
                this._port.RtsEnable = value;
            }
        }


        public bool CTS
        {
            get
            {
                return this._port.CtsHolding;
            }

        }

        public bool CD
        {
            get
            {
                return this._port.CDHolding;
            }
        }

        #endregion

        #region Methods
       
        public void WriteWithStopCharacters(string msg)
        {
            this._port.Write(msg + this.StopCharacters);
        }

        public void WriteData(byte [] data)
        {
            this._port.Write(data, 0, data.Count());
        }

        public void Open()
        {
            this._port.Open();
        }

        public void Close()
        {
            this._port.Close();
        }
        public bool IsOpen
        {
            get
            {
                return this._port.IsOpen;
            }

        }

        #endregion

        #region Events
        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[this._port.ReadBufferSize];

            int bytesRead = this._port.Read(buffer, 0, buffer.Length);

            this.temporaryString += Encoding.ASCII.GetString(buffer, 0, bytesRead);

            if(temporaryString.IndexOf(this.StopCharacters) > -1)
            {
                this.temporaryString = this.temporaryString.Remove(this.temporaryString.IndexOf(this.StopCharacters));

                DataReceived(this.temporaryString);

                this.temporaryString = string.Empty;
            }
            
        }

        public delegate void DataReceivedHandler(string msg);
        public event DataReceivedHandler DataReceived;
        #endregion

    }


}
