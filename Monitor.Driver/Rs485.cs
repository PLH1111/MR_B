using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Monitor.Common;

namespace Monitor.Driver
{
    public class Rs485: ITransmitPort
    {
        private SerialPort _port = new SerialPort();
        public  int        ReadTimeout  { get; set; } = 2000;
        public  int        WriteTimeout { get; set; } = 1000;
        public  int        DataBits     { get; set; } = 8;
        public  StopBits   StopBits     { get; set; } = StopBits.One;
        public  Parity     Parity       { get; set; } = Parity.None;
        public  string     PortParam    { get; set; } = "COM1,9600";

        public void Write<T>(T t)
        {
            try
            {
                if (typeof(T) != typeof(byte[])) return;
                
                var buffer = (byte[]) Convert.ChangeType(t, typeof(byte[]));
                
                if (buffer == null) return;

                if (_port == null || !_port.IsOpen) return;

                _port.Write(buffer, 0, buffer.Length);

                if (buffer.Length != 0)
                {
                    string str = string.Join(" ", buffer.Select(p => p.ToString("X2")));

                    LogHelper.Info($"Write: {str}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T Read<T>()
        {
            try
            {
                if (_port == null || !_port.IsOpen) return default(T);

                if (typeof(T) != typeof(byte[])) return default(T);
               
                int count = _port.BytesToRead;

                var data = new byte[count];

                _port.Read(data, 0, count);

                if (count != 0)
                {
                    string str = string.Join(" ", data.Select(p => p.ToString("X2")));

                    LogHelper.Info($" Read: {str}");
                }

                return (T)Convert.ChangeType(data, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Open()
        {
            try
            {
                if (_port == null) _port = new SerialPort();
              
                if (_port.IsOpen) return;
               
                string[] param = PortParam.Split(',');

                if (param.Length < 2) throw new Exception($"param.Length < 2!  {PortParam}");
              
                if (!int.TryParse(param[1], out var baudRate)) throw new Exception("baudRate error!");
                
                //_port.ReadTimeout
                //_port.NewLine
                _port.PortName     = param[0];
                _port.BaudRate     = baudRate;
                _port.DataBits     = DataBits;
                _port.StopBits     = StopBits;
                _port.Parity       = Parity;
                _port.ReadTimeout  = ReadTimeout;
                _port.WriteTimeout = WriteTimeout;
                _port.DtrEnable    = true;
                _port.ReceivedBytesThreshold = 1;
                //_port.ErrorReceived += (sender, args) => { Reset();};
                //_port.DataReceived += (sender, args) => { ReadDataToBuffer(); };
                _port.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Open serial port error: " + ex.Message);
            }
        }

        private Parity GetParity(string parity)
        {
            switch (parity)
            {
                case "EVEN":
                    return Parity.Even;
                case "ODD":
                    return Parity.Odd;
                case "NONE":
                    return Parity.None;
                default:
                    return Parity.None;
            }
        }

        public void ClearBuffer()
        {
            if (_port == null) return;
            if (!_port.IsOpen) return;
            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();
        }

        public void Close()
        {
            try
            {
                if (_port != null && _port.IsOpen) _port.Close();
            }
            catch (Exception ex)
            {
                _port = null;
                throw new Exception("SerialPort close failed！" + ex.Message);
            }
            finally
            {
                //_port?.Close();
            }
        }

        public void ChangeBaudrate(int baudrate)
        {
            Close();

            PortParam = $"{PortParam.Split(',')[0]},{baudrate}";

            Open();
        }
    }
}
