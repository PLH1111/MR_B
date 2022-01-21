using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public class CustomEnum
    {
        private readonly Dictionary<int, string> _dict = new Dictionary<int, string>();
        public string GetEnumValue(int data)
        {
            return _dict[data];
        }

        public CustomEnum(Dictionary<int, string> dict)
        {
            _dict = dict;
        }
    }
}
