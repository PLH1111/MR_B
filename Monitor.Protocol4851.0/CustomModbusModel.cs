using Monitor.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monitor.Protocol4851._0
{
    public class CustomModbusModel : IModbusModel
    {
        public int    Id                { get; set; }
        public byte   BcuAddress        { get; set; }
        public byte   BmuAddress        { get; set; }
        public byte   FunctionCode      { get; set; }
        public ushort RegisterAddress   { get; set; }
        public byte   SubCommand        { get; set; }
        public byte   Attach1Byte1      { get; set; }
        public byte   Attach1Byte2      { get; set; }
        public byte   Attach1Byte3      { get; set; }
        public byte   Attach1Byte4      { get; set; }
        public byte   Attach2Byte1      { get; set; }
        public byte   Attach2Byte2      { get; set; }
        public byte   Attach2Byte3      { get; set; }
        public byte   Attach2Byte4      { get; set; }
        public ushort DataNumber        { get; set; }
        public byte[] SendData          { get; private set; } = new byte[0];
        public byte[] ReceiveValidBytes { get; private set; } = new byte[0];

        private const int  ReadMiniLength       = 5;
        private const int  WriteResponseLength  = 17;
        public       bool  ReadMode             = true;

        public void InitSendBytes(bool isRead, byte[] data = null)
        {
            ReadMode = isRead;

            if (data != null)
            {
                DataNumber = (ushort)data.Length;
            }
            var list = new List<byte>
            {
                BcuAddress,
                FunctionCode,
                (byte) (((RegisterAddress & 0xFF00) >> 8)),
                (byte) (RegisterAddress & 0xff),
                SubCommand,
                Attach1Byte1,
                Attach1Byte2,
                Attach1Byte3,
                Attach1Byte4,
                Attach2Byte1,
                Attach2Byte2,
                Attach2Byte3,
                Attach2Byte4,
                (byte)((DataNumber & 0xff00) >> 8),
                (byte)(DataNumber & 0xff)
            };

            if (data != null)
            {
                list.AddRange(data);
            }

            list.AddRange(CrcHelper.GetCrc16(list.ToArray()));

            SendData = list.ToArray();
        }

        public bool CheckReceive(byte[] receive, out string result)
        {
            if (receive.Length < ReadMiniLength)
            {
                result = $"data length < {ReadMiniLength}";
                return false;
            }

            if (receive[0] != BcuAddress)
            {
                result = "Bcu address error!";
                return true;
            }

            //Error code 
            if (receive[1] == FunctionCode + 0x80)
            {
                result = $"Response error: {receive[2]:X2} {(ErrorCodeEnum)receive[2]}!";
                return true;
            }

            if (receive[1] != FunctionCode)
            {
                result = "Function code error!";
                return true;
            }

            int length = 0;

            if (ReadMode)
            { 
                length = receive[2] << 8 | receive[3];

                if (receive.Length < length + 7)
                {
                    result = $"data length less {length}";
                    return false;
                }

                receive = receive.Take(length + 7).ToArray();

            }
            else
            {
                if ((receive[2] << 8 | receive[3]) != (BmuAddress << 2) + RegisterAddress)
                {
                    result = "Register address error!";
                    return true;
                }

                if (receive.Length < WriteResponseLength)
                {
                    result = $"data length less {WriteResponseLength}";
                    return false;
                }

                receive = receive.Take(WriteResponseLength).ToArray();

                if (!CheckAttachData(receive, 5, 8))
                {
                    result = "Attach data error!";
                    return true;
                }

                if (DataNumber != (receive[13] <<8 | receive[14]))
                {
                    result = "Packet number error!";
                    return true;
                }
            }

            if (receive[4] != SubCommand)
            {
                result = "SubCommand error!";
                return true;
            }

            var calcCrc = CrcHelper.GetCrc16(receive.Take(receive.Length - 2).ToArray());

            var sourceCrc = receive.Skip(receive.Length - 2).Take(2).ToArray();

            if (!Enumerable.SequenceEqual(calcCrc, sourceCrc))
            {
                result = "Check crc error!";
                return true;
            }

            if (ReadMode)
            {
                ReceiveValidBytes = receive.Skip(5).Take(length).ToArray();
            }

            result = "OK";

            return true;
        }

        private bool CheckAttachData(byte[] data, int offset, int len)
        {
            if (offset < 0 || len < 0 || offset + len > data.Length) throw new ArgumentOutOfRangeException();

            data = data.Skip(offset).Take(len).ToArray();

            if (data.Length  < 8) return false;
            if (Attach1Byte1 != data[0]) return false;
            if (Attach1Byte2 != data[1]) return false;
            if (Attach1Byte3 != data[2]) return false;
            if (Attach1Byte4 != data[3]) return false;
            if (Attach2Byte1 != data[4]) return false;
            if (Attach2Byte2 != data[5]) return false;
            if (Attach2Byte3 != data[6]) return false;
            if (Attach2Byte4 != data[7]) return false;
            return true;
        }
    }
}