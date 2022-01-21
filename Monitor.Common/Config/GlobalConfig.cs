using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public class GlobalConfig
    {
        public DebugConfig   Bcu             { set; get; }
        public DebugConfig   Bmu             { set; get; }
        public DebugConfig   ProductInfo     { set; get; }
        public DebugConfig   RDB             { set; get; }
        public CommConfig    Communication   { get; set; }
        public UpgradeConfig UpgradeConfig   { set; get; }
        public GlobalConfig()
        {
            Bcu = new DebugConfig();

            Bmu = new DebugConfig();

            ProductInfo = new DebugConfig();

            Communication = new CommConfig();

            UpgradeConfig = new UpgradeConfig();
        }

    }

}
