using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public interface IIdAndValue
    {
        byte   Id    { get; set; }

        ushort Value { get; set; }
    }
}
