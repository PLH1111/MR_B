using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public class DebugConfig: ICloneable
    {
        [TypeConverter(typeof(HexTypeConverter))]
        public byte SubCommand { set; get; }
        public ushort RegisterAddress { set; get; }
        public ushort DataNumber { set; get; }
        [TypeConverter(typeof(HexTypeConverter))]
        public byte Attach1Byte1 { set; get; }
        [TypeConverter(typeof(HexTypeConverter))]
        public byte Attach1Byte2 { set; get; }
        [TypeConverter(typeof(HexTypeConverter))]
        public byte Attach1Byte3 { set; get; }
        [TypeConverter(typeof(HexTypeConverter))]
        public byte Attach1Byte4 { set; get; }
        [TypeConverter(typeof(HexTypeConverter))]
        public byte Attach2Byte1 { set; get; }
        [TypeConverter(typeof(HexTypeConverter))]
        public byte Attach2Byte2 { set; get; }
        [TypeConverter(typeof(HexTypeConverter))]
        public byte Attach2Byte3 { set; get; }
        [TypeConverter(typeof(HexTypeConverter))]
        public byte Attach2Byte4 { set; get; }

        public int ParallelNum { set; get; }
        public List<BmsInfo> BmsInfos { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    
}
