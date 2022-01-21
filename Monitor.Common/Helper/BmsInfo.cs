using System;

namespace Monitor.Common
{
    [Serializable]
    public class BmsInfo: IDisplay, ICloneable
    {
        public bool         WriteEnable { get; set; }  = false;
        public bool         Enable      { get; set; }  = true;
        public bool         Log         { get; set; }  = false;
        public string       Comment     { get; set; }  = string.Empty;
        public string       Name        { get; set; }  = string.Empty;
        public string       Value       { get; set; }  = string.Empty;
        public float        DownLimit   { get; set; }  = 0;
        public float        UpLimit     { get; set; }  = 0;
        public string       Unit        { get; set; }  = string.Empty;
        public int          StartByte   { get; set; }
        public int          ByteLength  { get; set; }
        public int          StartBit    { get; set; }
        public int          BitLength   { get; set; }
        public double       Factor      { get; set; } = 1;
        public double       Offset      { get; set; } = 0;
        public ByteOrder    ByteOrder   { get; set; } = ByteOrder.MSB;
        public string       DataType    { get; set; } = "UINT";
        public int          Decimal     { get; set; }
        public int          DisplayNo   { get; set; } = 0;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public enum DataTypeEnum
    {

    }
}