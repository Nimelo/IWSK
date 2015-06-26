using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class SerialPortWrapper
    {
        #region Fields & Properties

        private SerialPort _port;

        #endregion

        #region Ctors & DCtors

        public SerialPortWrapper()
        {
            this._port = new SerialPort();
        }

        ~SerialPortWrapper()
        {
            if(this._port.IsOpen)
                this._port.Close();
        }
        #endregion

        #region Configuration Properties
        public string PortName
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

        public int PortBaudRate
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

        public Parity PortParity
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

        public int PortDataBits
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

        public StopBits PortStopBits
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

        public Handshake PortHandShake
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

        public int PortReadTimeout
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

        public int PortWriteTimeout
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

        public string Read()
        {
            try
            {
                string message = _port.ReadLine();
                return message;
            }
            catch(TimeoutException te)
            {
                throw te;
            }
        }
        public void Write(string msg)
        {
            this._port.Write(msg);
        }

        public void Open()
        {
            this._port.Open();
        }

        public void Close()
        {
            this._port.Close();
        }
        public bool IsOpen()
        {
            return this._port.IsOpen;
        }

        #endregion

        #region Events

        #endregion

    }
}
