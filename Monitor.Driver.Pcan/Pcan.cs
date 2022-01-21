using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monitor.Common;

namespace Monitor.Driver
{
    using TPCANHandle = System.UInt16;

    public class Pcan : ITransmitPort
    {
        public string PortParam { get; set; } = "USB1,500";

        public void ChangeBaudrate(int baudrate)
        {
            ArrayList arrayList = new ArrayList();

            ConcurrentBag<TPCANMsg> tPCANMsgs = new ConcurrentBag<TPCANMsg>();
        }

        public void ClearBuffer()
        {
            
        }

        public void Close()
        {
            List<byte> buffer = new List<byte>();
        }

        public void Open()
        {
            PCANBasic.Initialize(0X51, TPCANBaudrate.PCAN_BAUD_500K);
        }

        public T Read<T>()
        {
            throw new NotImplementedException();
        }

        public void Write<T>(T t)
        {
            
        }
    }
}
