using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monitor.Common
{
    public class ParseByteUnitityHelper
    {
        public static byte[] PackageMessages(List<BmsInfo> profile)
        {
            byte[] canData = new byte[0];
            List<byte> allBytes = new List<byte>();
            foreach (var item in profile)
            {
                List<byte> list = new List<byte>();
                switch (item.DataType)
                {
                    case "ASCII":
                        {
                            canData = Encoding.ASCII.GetBytes(item.Value);
                            list = canData.ToList();
                            for (int i = 0; i < item.ByteLength - canData.Length; i++)
                            {
                                list.Add(0x00);
                            }
                            break;
                        }
                    case "HEX":
                        {
                            canData = UnitityHelper.StringToHexByte(item.Value);
                            list = canData.ToList();
                            for (int i = 0; i < item.ByteLength - canData.Length; i++)
                            {
                                list.Add(0x00);
                            }
                            break;
                        }
                    case "DATE":
                        {
                            var ins = item.Value.Split('/').ToList().Select(int.Parse).ToList();
                            if (ins.Count < 3)
                            {
                                return canData;
                            }
                            ins[0] -= 2000;
                            list.AddRange(ins.Select(p => (byte)p));
                            list.Add(0);
                            break;
                        }
                    case "DATETIME":
                        {
                            try
                            {
                                string[] str1 = item.Value.Split('/');
                                byte year = (byte)(int.Parse(str1[0]) - 2000);
                                byte month = byte.Parse(str1[1]);
                                string[] str2 = str1[2].Split(' ');
                                byte day = byte.Parse(str2[0]);
                                string[] str3 = str2[1].Split(':');
                                byte hour = byte.Parse(str3[0]);
                                byte min = byte.Parse(str3[1]);
                                byte sec = byte.Parse(str3[2]);
                                list.Add((byte)(((year & 0x3F) << 2) | ((month & 0x0C) >> 2)));
                                list.Add((byte)(((month & 0x03) << 6) | ((day & 0x1F) << 1) | ((hour & 0x10) >> 4)));
                                list.Add((byte)(((hour & 0x0F) << 4) | ((min & 0x3C) >> 2)));
                                list.Add((byte)(((min & 0x03) << 6) | (sec & 0x3F)));
                            }
                            catch (Exception)
                            {
                                MessageBox.Show(@"Date input error", @"tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw;
                            }
                            break;
                        }
                    case "VXX":
                        {
                            try
                            {
                                string[] str1 = item.Value.Split('V');
                                string[] str2 = str1[1].Split('.');
                                list.AddRange(str2.Select(byte.Parse));
                            }
                            catch (Exception)
                            {
                                MessageBox.Show(@"Version input error", @"tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;
                        }
                    default:
                        {
                            var temp = (ulong)Math.Round((Convert.ToDouble(item.Value) / item.Factor) - item.Offset, 0,
                                MidpointRounding.AwayFromZero);
                            temp = UnitityHelper.ReverseBytes64(temp << (64 - item.BitLength - item.StartBit));
                            canData = BitConverter.GetBytes(temp);
                            list = canData.ToList().Take(item.ByteLength).ToList();
                            break;
                        }
                }
                if (allBytes.Count - item.ByteLength == item.StartByte)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        allBytes[item.StartByte + i] += list[i];
                    }
                }
                else
                {
                    allBytes.AddRange(list);
                }
                //allBytes.AddRange(unitBytes);
            }
            return allBytes.ToArray();
        }

        private static string ParsedByte(byte[] data, BmsInfo canProfile)
        {
            if (canProfile.DataType == "BIT")
            {

            }
            ulong tempData;
            ulong dataU64;
            long data64;
            string value;
            byte[] resourceData = (byte[])data.Clone();
            if (data.Length < 8)
            {
                var list = data.ToList();
                list.AddRange(new byte[8 - data.Length]);
                data = list.ToArray();
            }
            switch (canProfile.ByteOrder)
            {
                //Little Endian----LSB
                case ByteOrder.LSB:
                    tempData = BitConverter.ToUInt64(data, 0);
                    //_data = (_tempData << canProfile.StartBit) >> (64 - canProfile.BitLength);
                    dataU64 = (tempData << (64 - canProfile.StartBit - canProfile.BitLength)) >>
                        (64 - canProfile.BitLength);
                    break;
                //Big Endian----MSB
                case ByteOrder.MSB:
                    tempData = BitConverter.ToUInt64(data, 0);
                    switch (canProfile.DataType)
                    {
                        case "BIT":
                            List<byte> tempList = resourceData.ToList();
                            tempList.InsertRange(0, new byte[8 - resourceData.Length]);
                            tempData = BitConverter.ToUInt64(tempList.ToArray(), 0);
                            tempData = UnitityHelper.ReverseBytes64(tempData);
                            dataU64 = (tempData << 64 - canProfile.StartBit - canProfile.BitLength) >> (64 - canProfile.BitLength);
                            break;
                        case "INT8":
                            dataU64 = (tempData << (64 - canProfile.StartBit - canProfile.BitLength)) >>
                                (64 - canProfile.BitLength);
                            break;
                        default:
                            tempData = UnitityHelper.ReverseBytes64(tempData);
                            dataU64 = (tempData << canProfile.StartBit) >> (64 - canProfile.BitLength);
                            break;
                    }
                    break;
                //MSB
                default:
                    tempData = BitConverter.ToUInt64(data, 0);
                    tempData = UnitityHelper.ReverseBytes64(tempData);
                    dataU64 = (tempData << canProfile.StartBit) >> (64 - canProfile.BitLength);
                    break;
            }
            switch (canProfile.DataType)
            {
                case "INT8":
                    data64 = (sbyte)dataU64;
                    value = Math.Round((data64 + canProfile.Offset) * canProfile.Factor, canProfile.Decimal,
                        MidpointRounding.AwayFromZero).ToString($"F{canProfile.Decimal}");
                    break;
                case "INT16":
                    data64 = (short)dataU64;
                    value = Math.Round((data64 + canProfile.Offset) * canProfile.Factor, canProfile.Decimal,
                        MidpointRounding.AwayFromZero).ToString($"F{canProfile.Decimal}");
                    break;
                case "INT32":
                    data64 = (int)dataU64;
                    value = Math.Round((data64 + canProfile.Offset) * canProfile.Factor, canProfile.Decimal,
                        MidpointRounding.AwayFromZero).ToString($"F{canProfile.Decimal}");
                    break;
                case "INT64":
                    data64 = (long)dataU64;
                    value = Math.Round((data64 + canProfile.Offset) * canProfile.Factor, canProfile.Decimal,
                        MidpointRounding.AwayFromZero).ToString($"F{canProfile.Decimal}");
                    break;
                case "UINT":
                    value = Math
                        .Round((dataU64 + canProfile.Offset) * canProfile.Factor, canProfile.Decimal,
                            MidpointRounding.AwayFromZero).ToString($"F{canProfile.Decimal}");
                    break;
                case "ASCII":
                    //var asciiData = BitConverter.GetBytes(dataU64);
                    //_value = Encoding.ASCII.GetString(getData).Replace("\0", "");
                    value = Encoding.ASCII.GetString(data).Trim('\0');
                    break;
                case "HEX":
                    //var hexData = BitConverter.GetBytes(_data); 
                    value = dataU64.ToString($"X{canProfile.ByteLength}").PadLeft(canProfile.BitLength / 4, '0').ToUpper()
                        .Substring(0, canProfile.BitLength / 4);
                    break;
                case "BOOL":
                    value = Math.Round((dataU64 + canProfile.Offset) * canProfile.Factor, canProfile.Decimal,
                        MidpointRounding.AwayFromZero).ToString($"F{canProfile.Decimal}");
                    break;
                case "COLOR":
                    value = Math.Round((dataU64 + canProfile.Offset) * canProfile.Factor, canProfile.Decimal,
                        MidpointRounding.AwayFromZero).ToString($"F{canProfile.Decimal}");
                    break;
                case "VXXXX":
                    value = $"V{data[0]}.{data[1]}.{(data[2] << 8 | data[3]).ToString().PadLeft(2, ' ')}";
                    break;
                case "RXXXX":
                    value = $"R{data[0]}.{data[1]}.{(data[2] << 8 | data[3]).ToString().PadLeft(2, ' ')}";
                    break;
                case "RXX":
                    value = $"R{data[0]}.{data[1]}";
                    break;
                case "VXX":
                    value = $"V{data[0]}.{data[1]}";
                    break;
                case "DATE":
                    value = $@"{data[0] + 2000}/{data[1]}/{data[2]}";
                    break;
                case "DATETIME":
                    value =
                        $@"{dataU64 << 32 >> (64 - 6)}/{dataU64 << 38 >> (64 - 4)}/{dataU64 << 42 >> (64 - 5)} {dataU64 << 47 >> (64 - 5)}:{dataU64 << 52 >> (64 - 6)}:{dataU64 << 58 >> (64 - 6)}";
                    break;
                default:
                    value = Math.Round((dataU64 + canProfile.Offset) * canProfile.Factor, canProfile.Decimal,
                        MidpointRounding.AwayFromZero).ToString($"F{canProfile.Decimal}");
                    break;
            }
            //canProfile.Value = value;
            return value;
        }

        public static void Parsed485Byte(byte[] data, List<BmsInfo> bmsInfos)
        {
            foreach (var bmsInfo in bmsInfos)
            {
                if (bmsInfo.DataType == "BIT")
                {

                }

                byte[] newByte = new byte[bmsInfo.ByteLength];

                if (data.Length < bmsInfo.StartByte + bmsInfo.ByteLength)
                {
                    //MessageBox.Show(@"Data length is less than bmsInfo length", @"tips", MessageBoxButtons.OK,
                    //    MessageBoxIcon.Error);
                    return;
                }

                Array.Copy(data, bmsInfo.StartByte, newByte, 0, bmsInfo.ByteLength);

                bmsInfo.Value = ParsedByte(newByte, bmsInfo);
            }
        }


    }
}
