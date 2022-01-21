using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Monitor.Common;
using Monitor.Protocol4851._0;

namespace Monitor.Upgrade
{
    class Upgrade
    {
        private readonly Protocol       _protocol;
        private readonly Action<string> _refreshText;
        private readonly Action<float>  _refreshMasterProgress;
        private readonly Action<int>    _refreshSlaverProgress;
        private readonly Action<bool>   _refreshResult;

        public byte        BcuAddress  { get; set; }
        public byte        BmuAddress  { get; set; }
        public FileHelper  UpgradeFile { get; set; }
        public UpgradeMode Mode        { get; set; }

        public int StartTimeout        { get; set; } = 1000;
        public int StartTimes          { get; set; } = 30;
        public int TranPacketDelayTime { get; set; } = 20;
        public int ProcessInterval     { get; set; } = 2000;
        public int CheckProgressTimeout { get; set; } = 60000;


        public Upgrade(Protocol protocol, Action<string> refreshText, Action<float> refreshMasterProgress,
            Action<int>               refreshSlaverProgress, Action<bool> refreshResult)
        {
            _refreshText           = refreshText;
            _protocol              = protocol;
            _refreshMasterProgress = refreshMasterProgress;
            _refreshSlaverProgress = refreshSlaverProgress;
            _refreshResult         = refreshResult;
        }

        private void JumpToBoot()
        {
            _refreshText("Jump form application to boot loader. . .");
            _protocol.UpgradeWrite(BcuAddress, BmuAddress, 0xB0, new byte[8]);
        }

        private void StartToUpgrade()
        {
            try
            {
                _refreshText("Start to upgrade. . .");

                List<byte> data = new List<byte>();

                data.AddRange(BitConverter.GetBytes((uint)Mode).Reverse());

                data.AddRange(BitConverter.GetBytes(UpgradeFile.ValidSize).Reverse());

                ////超时处理
                //for (int i = 0; i < 5; i++)
                //{
                //    Thread.Sleep(1000);

                //    try
                //    {
                //        _protocol.UpgradeRead(BcuAddress, BmuAddress, 0xB1, data.ToArray(), 7, out var receive);

                //        break;
                //    }
                //    catch(Exception ex)
                //    {
                //        if(_protocol.Stop) throw new Exception(ex.Message);
                //    }

                //    if (i == 4)
                //    {
                //        _protocol.ChangeBaudrate(115200);
                //    }
                //}

                for (int i = 0; i < StartTimes; i++)
                {
                    _protocol.UpgradeRead(BcuAddress, BmuAddress, 0xB1, data.ToArray(), 7, out var receive);

                    if (receive.Length < 7)
                    {
                        throw new Exception("Receive data length less 7!");
                    }

                    var softVersion = $"V{receive[1]}.{receive[2]}.{receive[3] << 8 | receive[4]}";

                    _refreshText($"softVersion is {softVersion}");

                    var appMode = receive[5];

                    var judge = Mode == UpgradeMode.Local ? 0x22: 0x21;

                    if (appMode != (byte)judge) 
                    {
                        Thread.Sleep(StartTimeout);

                        continue;//
                    }

                    //_protocol.ChangeBaudrate(115200);

                    //_protocol.UpgradeRead(BcuAddress, BmuAddress, 0xB1, data.ToArray(), 7, out _);

                    return;
                }

                throw new Exception("Please check hardware connect is OK!");
            }
            catch (Exception e)
            {
                throw new Exception("Start to upgrade failed: " + e.Message);
            }
        }

        private void EraseFlash()
        {
            try
            {
                _refreshText("Erase flash. . .");

                List<byte> attachData = new List<byte>();

                attachData.AddRange(BitConverter.GetBytes(0xFFFFFFFE).Reverse());
                attachData.AddRange(BitConverter.GetBytes(2).Reverse());

                _protocol.UpgradeWrite(BcuAddress, BmuAddress, 0xB2, attachData.ToArray());
            }
            catch (Exception e)
            {
                throw new Exception("Erase flash failed: " + e.Message);
            }
        }

        private void TransferFile()
        {
            try
            {
                _refreshText("Transfer upgrade file. . .");

                for (int i = 0; i < UpgradeFile.Packet.Count; i++)
                {
                    var pack = UpgradeFile.Packet[i];

                    List<byte> attachData = new List<byte>();

                    attachData.AddRange(BitConverter.GetBytes(i).Reverse());
                    attachData.AddRange(BitConverter.GetBytes(0).Reverse());
                    //attachData.AddRange(BitConverter.GetBytes(pack.Count).Reverse());

                    try
                    {
                        _protocol.UpgradeWrite(BcuAddress, BmuAddress, 0xB3, attachData.ToArray(), pack.ToArray());
                    }
                    catch(Exception ex)
                    {
                        throw new Exception($"Transfer pack {i + 1} failed: " + ex.Message);
                    }

                    _refreshMasterProgress((float)(i + 1) / UpgradeFile.Packet.Count * 100);

                    Thread.Sleep(TranPacketDelayTime);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Transfer upgrade file failed: " + e.Message);
            }
        }

        private void TransferFinish()
        {
            try
            {
                _refreshText("Transfer upgrade file finished and verify file. . .");

                List<byte> data = new List<byte>();

                data.AddRange(BitConverter.GetBytes(UpgradeFile.WholeSize).Reverse());
                data.AddRange(BitConverter.GetBytes(UpgradeFile.WholeCrc).Reverse());

                _protocol.UpgradeWrite(BcuAddress, BmuAddress, 0xB4, data.ToArray());
            }
            catch (Exception e)
            {
                throw new Exception("Transfer upgrade file finished and verify file failed: " + e.Message);
            }

        }

        private void RequestProgress()
        {
            try
            {
                _refreshText("Transfer progress of BMS internal transmission. . .");

                DateTime dt = DateTime.Now;

                while ((DateTime.Now - dt).TotalMilliseconds < CheckProgressTimeout)
                {
                    Thread.Sleep(ProcessInterval);

                    byte[] receive = new byte[2];

                    try
                    {
                        _protocol.UpgradeRead(BcuAddress, BmuAddress, 0xB5, new byte[8], 2, out receive);

                        if (receive.Length < 2)
                        {
                            throw new Exception("Receive data length less 2!");
                        }

                        _refreshSlaverProgress(receive[1]);

                        if (receive[1] == 100) return;
                    }
                    catch
                    {
                        //
                    }

                    if (_protocol.Stop) throw new Exception("Artificial stop!");

                    if (receive[1] == 0xFE) throw new Exception("Upgrade failed!");
                }

                throw new Exception("Upgrade timeout!");
            }
            catch (Exception e)
            {
                throw new Exception("Transfer progress of BMS internal transmission failed: " + e.Message);
            }
        }

        internal void UpgradeThread()
        {
            try
            {
                //JumpToBoot();

                StartToUpgrade();

                EraseFlash();

                TransferFile();

                TransferFinish();

                RequestProgress();

                _refreshResult(true);
            }
            catch (Exception e)
            {
                _refreshText(e.Message);

                _refreshResult(false);
            }
        }
    }

    enum UpgradeMode : uint
    {
        Local  = 0x4D4F4341,
        Remote = 0x52454D4F
    }
}