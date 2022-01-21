using Monitor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Protocol4851._0
{
    public class Bms
    {
        public List<BmsInfo> BmsInfos { get; }

        public List<Bcu> Bcus { get; }

        public EventHandler ReadMonitorReadData
        {
            set { Bcus.ForEach(p => p.ReadMonitorReadData = value); }
        }

        public Bms(GlobalConfig monitorInfo)
        {
            Bcus = new List<Bcu>();

            for (int i = 0; i < monitorInfo.Bcu.ParallelNum; i++)
            {
                Bcus.Add(new Bcu(monitorInfo, i));
            }
        }

        public void Refresh()
        {
            Bcus.ForEach(p => p.Refresh());
        }

        public void Save()
        {
            Bcus.ForEach(p => p.Save());
        }
        public override string ToString()
        {
            if (BmsInfos != null)
            {
                //LogHelper.Trace($"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{this}");
                LogHelper.Trace($",{this}");
            }
            return string.Join(",", BmsInfos.Select(p => p.Value));
        }
    }

    public class Bcu : IData
    {
        public List<BmsInfo> BmsInfos { get; }
        public DebugConfig debugConfig { get; }
        public int BcuIndex { get; }
        public List<Bmu> Bmus { get; }
        public int BmuIndex { get; }
        public EventHandler ReadMonitorReadData
        {
            get
            {
                return readMonitorReadData;
            }
            set
            {
                readMonitorReadData = value;
                Bmus.ForEach(p => p.ReadMonitorReadData = value);
            }
        }

        private EventHandler readMonitorReadData;

        public Bcu(GlobalConfig monitorInfo, int index)
        {
            debugConfig = monitorInfo.Bcu;

            BmsInfos = monitorInfo.Bcu.BmsInfos.Where(p => p.Enable).ToList();

            Bmus = new List<Bmu>();

            BcuIndex = index;

            BmuIndex = 0;

            for (int i = 0; i < monitorInfo.Bmu.ParallelNum; i++)
            {
                Bmus.Add(new Bmu(monitorInfo, index, i));
            }

            LogHelper.Debug($"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{Title()}");

            Clear();
        }

        public void Refresh()
        {
            try
            {
                ReadMonitorReadData(this, null);
            }
            catch
            {
                //Clear();
            }

            Bmus.ForEach(p => p.Refresh());
        }

        public void Clear()
        {
            BmsInfos.ForEach(P => P.Value = "0");
        }

        public override string ToString()
        {
            return string.Join(",", BmsInfos.Where(p => p.Log).Select(p => p.Value));
        }

        public string Title()
        {
            if (Bmus == null) return "";

            string str = "DateTime,";

            str += string.Join(",", BmsInfos.Where(p => p.Log).Select(p => p.Name));

            return str;
        }

        public void Save()
        {
            if (BmsInfos != null)
            {
                LogHelper.Debug($"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{this}");
            }
            //Bmus.ForEach(p => p.Save());
        }

    }

    public interface IData
    {
        List<BmsInfo> BmsInfos { get; }
        int BcuIndex { get; }
        int BmuIndex { get; }
        DebugConfig debugConfig { get; }
    }

    public class Bmu: IData
    {
        public List<BmsInfo> BmsInfos { get; }
        public DebugConfig debugConfig { get; }

        public int BcuIndex { get; }
        public List<Bmu> Bmus { get; }
        public int BmuIndex { get; }

        public EventHandler ReadMonitorReadData;

        public Bmu(GlobalConfig monitorInfo,int bcuIndex, int bmuindex)
        {
            BcuIndex = bcuIndex;

            BmuIndex = bmuindex;

            debugConfig = monitorInfo.Bmu;

            BmsInfos = monitorInfo.Bmu.BmsInfos.Where(p => p.Enable).ToList();
        }
        public void Refresh()
        {
            ReadMonitorReadData(this, null);
        }

        public override string ToString()
        {
            return string.Join(",", BmsInfos.Select(p => p.Value));
        }
        public void Save()
        {
            if (BmsInfos != null)
            {
                LogHelper.Trace($"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{this}");
            }
        }
    }

}
