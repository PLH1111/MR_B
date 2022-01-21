using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public class DataConfig
    {
        public int Num { set; get; }
        public int RegisterAddress { set; get; }


        public int RegisterNumber { set; get; }

        public List<BmsInfo> BmsInfos { get; set; }
    }
}
