using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Classes;

namespace UI
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Ctors
        public MainWindowViewModel()
        {
            this.Port = new Core.SerialPortWrapper();
            this.LogItems = new ObservableCollection<string>();
            this.TextInput = string.Empty;
            this.HexInput = string.Empty;
            this.Port.DataReceived += Port_DataReceived;


        }

        #endregion

        #region Properties & Fields

        private ObservableCollection<string> _logItems;

        public ObservableCollection<string> LogItems
        {
            get { return _logItems; }
            set
            {
                _logItems = value;
                this.Notify("LogItems");
            }
        }

        private string _textInput;

        public string TextInput
        {
            get { return _textInput; }
            set
            {
                _textInput = value;
                this.Notify("TextInput");
            }
        }

        private string _hexInput;

        public string HexInput
        {
            get { return _hexInput; }
            set
            {
                _hexInput = value;
                this.Notify("HexInput");
            }
        }

        private bool IsConfigured;
        #region Workers
        private BackgroundWorker readWorker;
        private BackgroundWorker writeWorker;
        #endregion
        #endregion

        #region Model
        private Core.SerialPortWrapper _port;

        public Core.SerialPortWrapper Port
        {
            get { return _port; }
            set
            {
                _port = value;
                this.Notify("Port");
            }
        }

        #endregion

        #region Methods
        private void Port_DataReceived(string msg)
        {
            App.Current.Dispatcher.Invoke(new Action(() => this.LogItems.Add(string.Format("Recived: \"{0}\"", msg))));
        }

        #endregion

        #region Commands

        #region DTR
        ICommand _dtr;
        public ICommand DTR
        {
            get
            {
                return _dtr ??
                    ( _dtr = new RelayCommand(SwitchDTR) );
            }
        }

        private void SwitchDTR(object obj)
        {
            this.Port.DTR = !this.Port.DTR;
            this.Notify("Port");
        }
        #endregion

        #region RTS
        ICommand _rts;
        public ICommand RTS
        {
            get
            {
                return _rts ??
                    ( _rts = new RelayCommand(SwitchRTS) );
            }
        }

        private void SwitchRTS(object obj)
        {
            this.Port.RTS = !this.Port.RTS;
            this.Notify("Port");
        }
        #endregion

        #region Open
        ICommand _openPort;
        public ICommand OpenPort
        {
            get
            {
                return _openPort ??
                    ( _openPort = new RelayCommand(OpenPortClick) );
            }
        }

        private void OpenPortClick(object obj)
        {
            if(this.Port.IsOpen && !this.IsConfigured)
            {
                this.LogItems.Add("Port is already used!");
            }
            else
            {
                this.IsConfigured = true;
                this.Port.Open();
                this.LogItems.Add("Just Opened port " + Port.Name);
                this.LogItems.Add("Started listening");
            }
            this.Notify("Port");
        }

        #endregion

        #region RefreshLines
        ICommand _refreshLines;
        public ICommand RefreshLines
        {
            get
            {
                return _refreshLines ??
                    ( _refreshLines = new RelayCommand(RefreshLinesClick) );
            }
        }

        private void RefreshLinesClick(object obj)
        {
            this.Notify("Port");
        }
        #endregion

        #region WriteMsg
        ICommand _writeMsg;
        public ICommand WriteMsg
        {
            get
            {
                return _writeMsg ??
                    ( _writeMsg = new RelayCommand(WriteMsgClick) );
            }
        }

        private void WriteMsgClick(object obj)
        {
            if(this.Port.IsOpen && this.IsConfigured)
            {
                this.LogItems.Add(string.Format("Sending: \"{0}\"", this.TextInput));
                this.Port.WriteWithStopCharacters(this.TextInput);
            }
            else
            {
                this.LogItems.Add("Cannot send input, because of port error!");
            }
        }
        #endregion

        #region WriteHex
        ICommand _writeHex;
        public ICommand WriteHex
        {
            get
            {
                return _writeHex ??
                    ( _writeHex = new RelayCommand(WriteHexClick) );
            }
        }

        private void WriteHexClick(object obj)
        {
            var bytes = this.HexInput.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => (byte)Convert.ToInt32(x, 16)).ToArray();
            this.Port.WriteData(bytes);

        }
  
        #endregion

        #region PingPong
        ICommand _pingPong;
        public ICommand PingPong
        {
            get
            {
                return _pingPong ??
                    ( _pingPong = new RelayCommand(PingPongClick) );
            }
        }

        private void PingPongClick(object obj)
        {
            if(this.Port.IsOpen && this.IsConfigured)
            {
                this.LogItems.Add("Sending ping");
                this.Port.WriteWithStopCharacters("ping");
            }
            else
            {
                this.LogItems.Add("Cannot send ping, because of port error!");
            }
        }
        #endregion

        #endregion

        #region Events
        #endregion

        #region Notify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
