using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public class CommConfig
    {
        public string ComPort { get; set; } = "COM1";
        public int Baudrate { get; set; } = 9600;
    }
}
