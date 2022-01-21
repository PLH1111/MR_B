using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Monitor.Common;
using System.Windows.Forms;

namespace Monitor.Protocol4851._0
{
    public class Protocol
    {
        private readonly ITransmitPort _driver;

        public Protocol(ITransmitPort driver)
        {
            _driver = driver;
        }

        private const byte WriteStandFunction   = 0x10;
        private const byte ReadStandFunction    = 0x03;
        private const byte WriteSpecialFunction = 0x42;
        private const byte ReadSpecialFunction  = 0x41;

        public bool Stop        { set; get; } = false;
        public int  RetryTimes  { set; get; } = 3;
        public int  TimeOut     { set; get; } = 3000;

        public void Request(IModbusModel model, bool isRead, byte[] sendData, out byte[] receiveData)
        {
            model.InitSendBytes(isRead, sendData);

            _driver.ClearBuffer();

            if (Stop) throw new Exception("Artificial stop!");

            _driver.Write(model.SendData);

            Thread.Sleep(10);

            for (int i = 0; i < RetryTimes; i++)
            {
                var dt = DateTime.Now;

                var list = new List<byte>();

                while (DateTime.Now.Subtract(dt).TotalMilliseconds < TimeOut)
                {
                    if (Stop) throw new Exception("Artificial stop!");

                    Thread.Sleep(10);

                    byte[] data = _driver.Read<byte[]>();

                    list.AddRange(data);

                    if (!model.CheckReceive(list.ToArray(), out var result)) continue;

                    if (Stop) throw new Exception("Artificial stop!");

                    if (result != "OK") throw new Exception(result);

                    receiveData = model.ReceiveValidBytes;

                    return;
                }
            }

            if (Stop) throw new Exception("Artificial stop!");

            throw new Exception("Communication timeout!");
        }
       
        #region Upgrade

        public void UpgradeWrite(byte bcu, byte bmu, byte subCmd, byte[] attachBytes, byte[] sendBytes = null)
        {
            if (attachBytes.Length < 8)
            {
                throw new Exception("Upgrade_Write attachBytes length less 8");
            }

            IModbusModel model = new CustomModbusModel
            {
                BcuAddress = bcu,
                BmuAddress = bmu,
                Attach1Byte1 = attachBytes[0],
                Attach1Byte2 = attachBytes[1],
                Attach1Byte3 = attachBytes[2],
                Attach1Byte4 = attachBytes[3],
                Attach2Byte1 = attachBytes[4],
                Attach2Byte2 = attachBytes[5],
                Attach2Byte3 = attachBytes[6],
                Attach2Byte4 = attachBytes[7],
                SubCommand = subCmd,
                FunctionCode = WriteSpecialFunction,
                RegisterAddress = 0,
                DataNumber = 0,
            };

            Request(model, false, sendBytes, out _);
        }

        public void UpgradeRead(byte bcu, byte bmu, byte subCmd, byte[] attachBytes, byte dataNum, out byte[] receive)
        {
            if (attachBytes.Length < 8)
            {
                throw new Exception("Upgrade_Read attachBytes length less 8");
            }

            IModbusModel model = new CustomModbusModel
            {
                BcuAddress = bcu,
                BmuAddress = bmu,
                Attach1Byte1 = attachBytes[0],
                Attach1Byte2 = attachBytes[1],
                Attach1Byte3 = attachBytes[2],
                Attach1Byte4 = attachBytes[3],
                Attach2Byte1 = attachBytes[4],
                Attach2Byte2 = attachBytes[5],
                Attach2Byte3 = attachBytes[6],
                Attach2Byte4 = attachBytes[7],
                SubCommand = subCmd,
                FunctionCode = ReadSpecialFunction,
                RegisterAddress = 0,
                DataNumber = dataNum
            };

            Request(model, true, null, out receive);
        }
      
        #endregion

        #region Monitor

        public void MonitorRead(object sender, EventArgs e)
        {
            IData data = sender as IData;

            var config = data.debugConfig;

            IModbusModel model;

            List<byte> buffer = new List<byte>();

            ushort maxFrameReg = 256;

            if (data == null) return;

            ushort times = (ushort)((config.DataNumber + maxFrameReg - 1) / maxFrameReg);

            for (var i = 0; i < times; i++)
            {
                ushort length;

                if (i == times - 1)
                {
                    if (((ushort)config.DataNumber % maxFrameReg) > 0)
                    {
                        length = (ushort)(config.DataNumber % maxFrameReg);
                    }
                    else
                    {
                        length = maxFrameReg;
                    }
                }
                else
                {
                    length = maxFrameReg;
                }

                //model = new StandModbusModel
                //{
                //    BcuAddress = (byte)(0x65 + data.BcuIndex),
                //    BmuAddress = (byte)data.BmuIndex,
                //    FunctionCode = ReadStandFunction,
                //    RegisterAddress = (ushort)(config.RegisterAddress + i * maxFrameReg),
                //    RegisterNum = length,
                //};

                model = new CustomModbusModel
                {
                    BcuAddress = (byte)(0x65 + data.BcuIndex),
                    BmuAddress = (byte)data.BmuIndex,
                    Attach1Byte1 = config.Attach1Byte1,
                    Attach1Byte2 = config.Attach1Byte2,
                    Attach1Byte3 = config.Attach1Byte3,
                    Attach1Byte4 = config.Attach1Byte4,
                    Attach2Byte1 = config.Attach2Byte1,
                    Attach2Byte2 = config.Attach2Byte2,
                    Attach2Byte3 = config.Attach2Byte3,
                    Attach2Byte4 = config.Attach2Byte4,
                    SubCommand = config.SubCommand,
                    FunctionCode = ReadSpecialFunction,
                    DataNumber = length,
                    RegisterAddress = (ushort)(config.RegisterAddress + i * maxFrameReg),
                };

                Request(model, true, null, out var receive);

                receive = receive.Skip(1).Take(receive.Length - 1).ToArray();

                buffer.AddRange(receive);
            }

            ParseByteUnitityHelper.Parsed485Byte(buffer.ToArray(), data.BmsInfos);
        }

        public void MonitorWrite(int bcuIndex, ushort registerAddress, byte[] data)
        {
            IModbusModel model = new StandModbusModel
            {
                BcuAddress = (byte)(0x65 + bcuIndex),
                BmuAddress = 0,
                FunctionCode = WriteStandFunction,
                RegisterAddress = registerAddress,
                RegisterNum = 1
            };

            Request(model, false, data, out _);
        }

        #endregion

        #region Debug

        public void DebugWrite(byte bcu, byte bmu, byte subCmd, byte[] attachBytes, byte[] sendBytes = null)
        {
            if (attachBytes.Length < 8)
            {
                throw new Exception("Debug_Write attachBytes length less 8");
            }

            IModbusModel model = new CustomModbusModel
            {
                BcuAddress   = bcu,
                BmuAddress   = bmu,
                Attach1Byte1 = attachBytes[0],
                Attach1Byte2 = attachBytes[1],
                Attach1Byte3 = attachBytes[2],
                Attach1Byte4 = attachBytes[3],
                Attach2Byte1 = attachBytes[4],
                Attach2Byte2 = attachBytes[5],
                Attach2Byte3 = attachBytes[6],
                Attach2Byte4 = attachBytes[7],
                SubCommand   = subCmd,
                FunctionCode = WriteSpecialFunction,
                DataNumber   = 0,
                RegisterAddress = 0,
            };

            Request(model, false, sendBytes, out _);
        }

        public void DebugRead(byte bcu, byte bmu, byte subCmd, byte[] attachBytes, byte dataNum, out byte[] receive)
        {
            if (attachBytes.Length < 8)
            {
                throw new Exception("Debug_Read attachBytes length less 8");
            }

            IModbusModel model = new CustomModbusModel
            {
                BcuAddress   = bcu,
                BmuAddress   = bmu,
                Attach1Byte1 = attachBytes[0],
                Attach1Byte2 = attachBytes[1],
                Attach1Byte3 = attachBytes[2],
                Attach1Byte4 = attachBytes[3],
                Attach2Byte1 = attachBytes[4],
                Attach2Byte2 = attachBytes[5],
                Attach2Byte3 = attachBytes[6],
                Attach2Byte4 = attachBytes[7],
                SubCommand   = subCmd,
                FunctionCode = ReadSpecialFunction,
                DataNumber   = dataNum,
                RegisterAddress = 0,
            };

            Request(model, true, null, out receive);

            receive = receive.Skip(1).Take(receive.Length - 1).ToArray();
        }

        public void DebugWrite(DebugConfig debugConfig)
        {
            IModbusModel model = new CustomModbusModel
            {
                BcuAddress = 0x65,
                BmuAddress = 0,
                Attach1Byte1 = debugConfig.Attach1Byte1,
                Attach1Byte2 = debugConfig.Attach1Byte2,
                Attach1Byte3 = debugConfig.Attach1Byte3,
                Attach1Byte4 = debugConfig.Attach1Byte4,
                Attach2Byte1 = debugConfig.Attach2Byte1,
                Attach2Byte2 = debugConfig.Attach2Byte2,
                Attach2Byte3 = debugConfig.Attach2Byte3,
                Attach2Byte4 = debugConfig.Attach2Byte4,
                SubCommand = debugConfig.SubCommand,
                FunctionCode = WriteSpecialFunction,
                DataNumber = debugConfig.DataNumber,
                RegisterAddress = debugConfig.RegisterAddress,
            };

            var data = ParseByteUnitityHelper.PackageMessages(debugConfig.BmsInfos);

            Request(model, false, data, out _);
        }

        public void DebugRead(DebugConfig debugConfig)
        {
            IModbusModel model = new CustomModbusModel
            {
                BcuAddress = 0x65,
                BmuAddress = 0,
                Attach1Byte1 = debugConfig.Attach1Byte1,
                Attach1Byte2 = debugConfig.Attach1Byte2,
                Attach1Byte3 = debugConfig.Attach1Byte3,
                Attach1Byte4 = debugConfig.Attach1Byte4,
                Attach2Byte1 = debugConfig.Attach2Byte1,
                Attach2Byte2 = debugConfig.Attach2Byte2,
                Attach2Byte3 = debugConfig.Attach2Byte3,
                Attach2Byte4 = debugConfig.Attach2Byte4,
                SubCommand   = debugConfig.SubCommand,
                FunctionCode = ReadSpecialFunction,
                DataNumber = debugConfig.DataNumber,
                RegisterAddress = debugConfig.RegisterAddress,
            };

            Request(model, true, null, out var receive);

            receive = receive.Skip(1).Take(receive.Length - 1).ToArray();

            ParseByteUnitityHelper.Parsed485Byte(receive, debugConfig.BmsInfos);
        }

        public void Calibrate(object sender, EventArgsCalibrate e)
        {
            ICalibrateUiInfo calibrateUiInfo = sender as ICalibrateUiInfo;

            if (!int.TryParse(calibrateUiInfo.Input[e.Step], out var inputValue))
            {
                throw new Exception("input calibrate value change to int error!");
            }

            byte[] calibrateValue = BitConverter.GetBytes(inputValue).Reverse().ToArray();

            var attachBytes = new byte[8];

            attachBytes[0] = calibrateUiInfo.Channel;
            attachBytes[1] = (byte)(e.Step + 1);

            Array.Copy(calibrateValue, 0, attachBytes, 4, 4);

            Thread.Sleep(1000);

            DebugRead(0x65, 0, 0x55, attachBytes, 9, out var receive);

            switch (e.Step)
            {
                case 0:
                    //calibrateUiInfo.Output1 = BitConverter.ToInt32(receive.Reverse().ToArray(), 0).ToString();
                    break;
                case 1:
                    float gain = BitConverter.ToUInt32(receive.Reverse().ToArray(), 0);
                    float offset = BitConverter.ToUInt32(receive.Reverse().ToArray(), 4);
                    calibrateUiInfo.Result = new float[2] { gain, offset };
                    break;
            }
        }

        public void ReadAfe(object sender, EventArgs e)
        {
            IIdAndValue idAndValue = sender as IIdAndValue;

            byte[] attachBytes = new byte[8];

            IModbusModel model = new CustomModbusModel
            {
                BcuAddress   = 0x65,
                BmuAddress   = 0,
                Attach1Byte1 = attachBytes[0],
                Attach1Byte2 = attachBytes[1],
                Attach1Byte3 = attachBytes[2],
                Attach1Byte4 = idAndValue.Id,
                Attach2Byte1 = attachBytes[4],
                Attach2Byte2 = attachBytes[5],
                Attach2Byte3 = attachBytes[6],
                Attach2Byte4 = attachBytes[7],
                SubCommand   = 0xD0,
                FunctionCode = 0x41,
                RegisterAddress = 0,
                DataNumber   = 1,
            };

            Request(model, true, null, out var receive);

            receive = receive.Skip(1).Take(receive.Length - 1).ToArray();

            if (receive.Length > 1)
            {
                idAndValue.Value = BitConverter.ToUInt16(receive.Take(2).Reverse().ToArray(), 0);
            }
        }

        public void WriteAfe(object sender, EventArgs e)
        {
            IIdAndValue idAndValue = sender as IIdAndValue;

            byte[] attachBytes = new byte[8];

            var data = UnitityHelper.UnShortToBytes(idAndValue.Value);

            attachBytes[3] = idAndValue.Id;

            DebugWrite(0x65, 0, 0xD0, attachBytes, data);
        }

        #endregion

    }
}
