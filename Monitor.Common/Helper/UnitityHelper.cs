using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monitor.Common
{
    public static class UnitityHelper
    {
        #region BinaryToBytes

        /// <summary> 
        /// Trans binary file to list of byte
        /// path of binary file
        /// binary file length
        /// list of byte
        /// </summary>
        /// <param name="path"></param>
        /// <param name="length"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool BinaryToBytes(string path, out int length, out List<byte> data)
        {
            length = 0;
            data = null;
            if (File.Exists(path))
            {
                var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                var fi = new FileInfo(path);
                var br = new BinaryReader(fs);
                data = new List<byte>(br.ReadBytes((int)fi.Length));
                br.Close();
                fs.Close();
                length = (int)fi.Length;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] StringToHexByte(string value)
        {
            var datas = new byte[value.Length / 2];

            var j = 0;

            for (var i = 0; i < value.Length; i += 2)
            {
                datas[j++] = Convert.ToByte(value.Substring(i, 2), 16);
            }

            return datas;
        }
        #endregion

        #region ReverseTools
        /// 
        ///     reverse bit of Uint32
        /// 
        /// 
        /// 
        public static uint Reverse(uint x)
        {
            x = ((x & 0xaaaaaaaa) >> 1) | ((x & 0x55555555) << 1);
            x = ((x & 0xcccccccc) >> 2) | ((x & 0x33333333) << 2);
            x = ((x & 0xf0f0f0f0) >> 4) | ((x & 0x0f0f0f0f) << 4);
            x = ((x & 0xff00ff00) >> 8) | ((x & 0x00ff00ff) << 8);
            return (x >> 16) | (x << 16);
        }
        /// 
        ///     reverse bit of Uint64
        /// 
        /// 
        /// 
        public static ulong ReverseUint64(ulong x)
        {
            x = ((x & 0xaaaaaaaaaaaaaaaa) >> 1) | ((x & 0x5555555555555555) << 1);
            x = ((x & 0xcccccccccccccccc) >> 2) | ((x & 0x3333333333333333) << 2);
            x = ((x & 0xf0f0f0f0f0f0f0f0) >> 4) | ((x & 0x0f0f0f0f0f0f0f0f) << 4);
            x = ((x & 0xff00ff00ff00ff00) >> 8) | ((x & 0x00ff00ff00ff00ff) << 8);
            x = ((x & 0xffff0000ffff0000) >> 16) | ((x & 0x0000ffff0000ffff) << 16);
            return (x >> 32) | (x << 32);
        }
        /// 
        ///     reverse byte of Uint64
        /// 
        /// 
        /// 
        public static ulong ReverseBytes64(ulong value)
        {
            return ((value & 0x00000000000000FFUL) << 56) | ((value & 0x000000000000FF00UL) << 40) |
                   ((value & 0x0000000000FF0000UL) << 24) | ((value & 0x00000000FF000000UL) << 8) |
                   ((value & 0x000000FF00000000UL) >> 8) | ((value & 0x0000FF0000000000UL) >> 24) |
                   ((value & 0x00FF000000000000UL) >> 40) | ((value & 0xFF00000000000000UL) >> 56);
        }
        /// 
        ///     reverse byte of Uint32
        /// 
        /// 
        /// 
        public static uint ReverseBytes32(uint value)
        {
            return ((value & 0x000000FF) << 24) | ((value & 0x0000FF00) << 8) |
                   ((value & 0x00FF0000) >> 8) | ((value & 0xFF000000) >> 24);
        }
        /// 
        ///     reverse byte of Uint16
        /// 
        /// 
        /// 
        public static ushort ReverseBytes16(ushort value)
        {
            return (ushort)(((value & 0x00FF) << 8) | ((value & 0xFF00) >> 8));
        }

        public static ulong Rs485ByteToLong(byte[] data, int startByte, int byteLength)
        {
            var temp = 0;
            var dataNew = new byte[8];
            for (var i = startByte; i < startByte + byteLength; i++)
            {
                dataNew[temp] = data[i];
                temp++;
            }
            var value = BitConverter.ToUInt64(dataNew, 0);
            #region oldCoding
            //var dataNew = new List(buffer.Skip(startByte).Take(byteLength));
            //for (var i = byteLength + startByte - 1; i > startByte; i--) dataNew.Add(buffer[i]);
            //dataNew.Reverse();//
            //while (dataNew.Count < 8) dataNew.Insert(0, 0x00);
            //switch (byteLength)
            //{
            //    case 1:
            //        value = buffer[6 + startByte];
            //        break;
            //    case 2:
            //        value = Convert.ToUInt32((buffer[6 + startByte] & 0xff) | ((buffer[7 + startByte] << 8) & 0xff00));
            //        break;
            //    case 3:
            //        value = Convert.ToUInt32((buffer[6 + startByte] & 0xff) | ((buffer[7 + startByte] << 8) & 0xff00) | ((buffer[8 + startByte] << 16) & 0xff0000));
            //        break;
            //    case 4:
            //        value = BitConverter.ToUInt32(buffer, 6 + startByte);
            //        break;
            //}
            #endregion
            return value;
        }

        #endregion

        #region CalcKb
        public static float[] GetKb(float[][] data)
        {
            float[] result = null;

            if (data.Length < 2) return result;

            if (data[0].Length < 2) return result;

            var x1 = data[0][0];
            var y1 = data[0][1];
            var x2 = data[1][0];
            var y2 = data[1][1];

            if (x2 == x1)
            {
                throw new ArgumentException("Input argument error!");
            }

            var k = (y2 - y1) / (x2 - x1);
            var b = y1 - (k * x1);

            result = new float[2];

            result[0] = k;
            result[1] = b;

            return result;
        }

        #endregion

        /// <summary>
        /// 将字节数组在正常模式与小端模式切换,小端模式 -- 以Word为单位,低位在前,高位在后
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SwitchBytes(byte[] data)
        {
            if (data == null || data.Length < 2 || data.Length % 2 != 0)
            {
                return null;
            }
            byte[] bytes = new byte[data.Length];
            for (int i = 0; i < data.Length; i += 2)
            {
                bytes[i] = data[i + 1];
                bytes[i + 1] = data[i];
            }
            return bytes;
        }

        #region [Hexstr - Bytes] Converter

        /// <summary>
        /// 16进制字符串转成字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] HexstrToBytes(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            try
            {
                str = str.Replace(" ", "").Replace("\r", "").Replace("\n", "");
                if (str.Length % 2 == 1)
                {
                    str = str.PadLeft(str.Length + 1, '0');
                }
                byte[] bytes = new byte[str.Length / 2];
                for (int i = 0; i < str.Length; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
                }
                return bytes;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteExceptionLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 字节数组转成16进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToHexstr(byte[] data)
        {
            if (data == null || data.Length < 1)
            {
                return null;
            }
            try
            {
                StringBuilder sb = new StringBuilder(data.Length * 3);
                foreach (byte b in data)
                {
                    sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
                }
                return sb?.ToString()?.ToUpper()?.Trim();
            }
            catch (Exception ex)
            {
                //LogHelper.WriteExceptionLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 将字节数组转化成16进制的字符串
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns></returns>
        public static string ToHexstr2(byte[] data)
        {
            if (data == null || data.Length < 1)
            {
                return null;
            }
            try
            {
                StringBuilder sb = new StringBuilder(data.Length * 2);
                foreach (byte b in data)
                {
                    sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                }
                return sb?.ToString()?.ToUpper();
            }
            catch (Exception ex)
            {
                //LogHelper.WriteExceptionLog(ex);
                return null;
            }
        }

        public static string ByteToHexStr(byte data)
        {
            try
            {
                return Convert.ToString(data, 16).PadLeft(2, '0').ToUpper();
            }
            catch (Exception ex)
            {
                //LogHelper.WriteExceptionLog(ex);
                return null;
            }
        }

        public static byte HexStrToByte(string str)
        {
            try
            {
                return Convert.ToByte(str, 16);
            }
            catch (Exception)
            {
                return 0;
            }

        }


        #endregion

        #region [Int - Bytes] Converter

        /// <summary>
        /// 将int转化成字节数据
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="isLittleMode">是否小端模式</param>
        /// <returns>转化成功的字节数组</returns>
        public static byte[] IntToBytes(int value, bool isLittleMode = false)
        {
            try
            {
                var vs = BitConverter.GetBytes(value);
                if (isLittleMode == false)
                {
                    Array.Reverse(vs);
                }
                return vs;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteExceptionLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 将字节数组转化成int整数
        /// </summary>
        /// <param name="data">输入的字节数组</param>
        /// <param name="isLittleMode">是否是小端模式</param>
        /// <returns></returns>
        public static int ToInt(byte[] data, bool isLittleMode = false)
        {
            if (data == null || data.Length != 4)
            {
                return 0;
            }
            try
            {
                byte[] vs = new byte[4];
                data.CopyTo(vs, 0);
                if (isLittleMode == false)
                {
                    Array.Reverse(vs);
                }
                return BitConverter.ToInt32(vs, 0);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteExceptionLog(ex);
                return 0;
            }
        }

        #endregion

        #region [Uint - Bytes] Converter

        /// <summary>
        /// 将uint转化成字节数据
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="isLittleMode">是否小端模式</param>
        /// <returns>转化成功的字节数组</returns>
        public static byte[] UintToBytes(uint value, bool isLittleMode = false)
        {
            var vs = BitConverter.GetBytes(value);
            if (isLittleMode == false)
            {
                Array.Reverse(vs);
            }
            return vs;
        }

        /// <summary>
        /// 将字节数组转化成uint整数
        /// </summary>
        /// <param name="data">输入的字节数组</param>
        /// <param name="isLittleMode">是否是小端模式</param>
        /// <returns></returns>
        public static uint ToUint(byte[] data, bool isLittleMode = false)
        {
            if (data == null || data.Length != 4)
            {
                return 0;
            }
            try
            {
                byte[] vs = new byte[4];
                data.CopyTo(vs, 0);
                if (isLittleMode == false)
                {
                    Array.Reverse(vs);
                }
                return BitConverter.ToUInt32(vs, 0);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteExceptionLog(ex);
                return 0;
            }
        }

        #endregion

        #region [Short - Bytes] Converter


        /// <summary>
        /// short整数转化成字节数据
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="isLittleMode">是否小端模式</param>
        /// <returns>转化成功的字节数组</returns>
        public static byte[] ShortToBytes(short value, bool isLittleMode = false)
        {
            var vs = BitConverter.GetBytes(value);
            if (isLittleMode == false)
            {
                Array.Reverse(vs);
            }
            return vs;
        }

        /// <summary>
        /// short整数转化成字节数据
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="isLittleMode">是否小端模式</param>
        /// <returns>转化成功的字节数组</returns>
        public static byte[] ShortToBytes(object value, bool isLittleMode = false)
        {
            var vs = BitConverter.GetBytes(Convert.ToInt16(value));
            if (isLittleMode == false)
            {
                Array.Reverse(vs);
            }
            return vs;
        }


        /// <summary>
        /// 将字节数组转化成short整数
        /// </summary>
        /// <param name="data">输入的字节数组</param>
        /// <param name="isLittleMode">是否是小端模式</param>
        /// <returns>short整数</returns>
        public static short ToShort(byte[] data, bool isLittleMode = false)
        {
            if (data.Length != 2)
            {
                return 0;
            }
            try
            {
                byte[] vs = new byte[2];
                data.CopyTo(vs, 0);
                if (isLittleMode == false)
                {
                    Array.Reverse(vs);
                }
                return BitConverter.ToInt16(vs, 0);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteExceptionLog(ex);
                return 0;
            }
        }

        #endregion

        #region [Ushort - Bytes] Converter

        /// <summary>
        /// ushort整数转化成字节数据
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="isLittleMode">是否小端模式</param>
        /// <returns>转化成功的字节数组</returns>
        public static byte[] UnShortToBytes(ushort value, bool isLittleMode = false)
        {
            var vs = BitConverter.GetBytes(value);

            if (isLittleMode == false)
            {
                Array.Reverse(vs);
            }
            return vs;
        }


        /// <summary>
        /// ushort整数转化成字节数据
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="isLittleMode">是否小端模式</param>
        /// <returns>转化成功的字节数组</returns>
        public static byte[] UnShortToBytes(object value, bool isLittleMode = false)
        {
            var vs = BitConverter.GetBytes(Convert.ToUInt16(value));
            if (isLittleMode == false)
            {
                Array.Reverse(vs);
            }
            return vs;
        }


        /// <summary>
        /// 将字节数组转化成short整数
        /// </summary>
        /// <param name="data">输入的字节数组</param>
        /// <param name="isLittleMode">是否是小端模式</param>
        /// <returns>ushort整数</returns>
        public static ushort ToUnShort(byte[] data, bool isLittleMode = false)
        {
            if (data.Length != 2)
            {
                return 0;
            }
            try
            {
                byte[] vs = new byte[2];
                data.CopyTo(vs, 0);
                if (isLittleMode == false)
                {
                    Array.Reverse(vs);
                }
                return BitConverter.ToUInt16(vs, 0);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteExceptionLog(ex);
                return 0;
            }
        }

        #endregion

    }
}
