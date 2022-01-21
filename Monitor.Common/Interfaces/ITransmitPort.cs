using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public interface ITransmitPort
    {
        void Write<T>(T t);

        T Read<T>();

        void Open();

        void Close();

        string PortParam { get; set; }

        void ClearBuffer();

        void ChangeBaudrate(int baudrate);

    }
}
