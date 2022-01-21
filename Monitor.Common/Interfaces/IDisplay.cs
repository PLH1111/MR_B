using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public interface IDisplay
    {
        string Name { get; set; }
        string Comment { get; set; }
        string Value { get; set; }
        string DataType { get; set; }
        string Unit { get; set; }
        float UpLimit { get; set; }
        float DownLimit { get; set; }
    }
}
