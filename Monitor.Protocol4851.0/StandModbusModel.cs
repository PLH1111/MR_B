using System.Collections.Generic;
using System.Linq;
using Monitor.Common;

namespace Monitor.Protocol4851._0
{
    public class StandModbusModel: IModbusModel
    {
        public int    Id                { get; set; }
        public byte   BcuAddress        { get; set; }
        public byte   BmuAddress        { get; set; }
        public byte   FunctionCode      { get; set; }
        public ushort RegisterAddress   { get; set; }
        public ushort RegisterNum       { get; set; }
        public byte[] SendData          { get; private set; } = new byte[0];
        public byte[] ReceiveValidBytes { get; private set; } = new byte[0];

        private const int ReadMiniLength    = 5;
        public       bool  ReadMode         = true;

        public void InitSendBytes(bool isRead, byte[] data = null)
        {
            ReadMode = isRead;

            var list = new List<byte>
            {
                BcuAddress,
                FunctionCode,
                (byte) (BmuAddress << 2 + ((RegisterAddress & 0x0300) >> 8)),
                (byte) (RegisterAddress & 0xff),
                (byte) ((RegisterNum  & 0xff00) >> 8),
                (byte) (RegisterNum  & 0xff)
            };

            if (!isRead && data != null)
            {
                list.Add((byte)data.Length);
                list.AddRange(data);   
            }

            list.AddRange(CrcHelper.GetCrc16(list.ToArray()));

            SendData = list.ToArray();
        }

        public bool CheckReceive(byte[] receive, out string result)
        {
            byte[] calcCrc;
            byte[] sourceCrc;

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

            if (ReadMode)
            {
                if (receive[2] + ReadMiniLength > receive.Length)
                {
                    result = $"data length less {receive[2]}";

                    return false;
                }

                calcCrc = CrcHelper.GetCrc16(receive, 0, receive[2] + 3);

                ReceiveValidBytes = receive.Skip(3).Take(receive[2]).ToArray();

                sourceCrc = receive.Skip(receive[2] + ReadMiniLength - 2).Take(2).ToArray();
            }
            else
            {
                if (8 > receive.Length)
                {
                    result = $"data length less 8";

                    return false;
                }

                calcCrc = CrcHelper.GetCrc16(receive, 0, 6);

                sourceCrc = receive.Skip(6).Take(2).ToArray();

            }

            if (!Enumerable.SequenceEqual(calcCrc, sourceCrc))
            {
                result = "Check crc error!";

                return true;
            }

            //if ((receive[2] & 0xfc) != (BmuAddress << 2))
            //{
            //    result = "Bmu address error!";
            //}

            result = "OK";

            return true;
        }
    }
}
