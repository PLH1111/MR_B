using Monitor.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monitor.View
{
    public partial class FormRegisterRw : Form
    {
        public FormRegisterRw()
        {
            InitializeComponent();
        }

        public FormRegisterRw(GlobalConfig globalConfig) : this()
        {
            _globalConfig = globalConfig;

            for (int i = 0; i < 6; i++)
            {
                panel1.Controls.Add(new UcReadWriteBox() { WriteHandler = WriteAfe, ReadHandler = ReadAfe, Dock = DockStyle.Top });
            }

            comboBoxRdb.Items.AddRange(globalConfig.RDB.BmsInfos.Where(p => p.WriteEnable).Select(p => p.Name).ToArray());
        }

        public EventHandler ReadAfeHandler;

        public EventHandler WriteAfeHandler;

        public Action<DebugConfig> ReadDebugeHandler;

        public Action<DebugConfig> WriteDebugHandler;

        public Action<MessageHelper> TriggerDebug;

        private void WriteAfe(object sender, EventArgs e)
        {
            TriggerDebug(new MessageHelper(sender, e, WriteAfeHandler));
        }

        private void ReadAfe(object sender, EventArgs e)
        {
            TriggerDebug(new MessageHelper(sender, e, ReadAfeHandler));
        }

        private void buttonWriteRdb_Click(object sender, EventArgs e)
        {
            if (comboBoxRdb.SelectedIndex == -1) return;

            var list = _globalConfig.RDB.BmsInfos.Where(p => p.WriteEnable).ToList();

            var bmsInfo = (BmsInfo)list[comboBoxRdb.SelectedIndex].Clone();

            bmsInfo.Value = textBoxRdb.Text;

            var debugConfig = (DebugConfig)(_globalConfig.Bcu.Clone());

            debugConfig.RegisterAddress = (ushort)bmsInfo.StartByte;

            bmsInfo.StartByte = 0;

            debugConfig.DataNumber = (ushort)bmsInfo.ByteLength;

            debugConfig.BmsInfos = new List<BmsInfo>() { bmsInfo };

            TriggerDebug(new MessageHelper(null, null, (s, args) =>
            {
                WriteDebugHandler(debugConfig);
            }));
        }

        private void buttonReadRdb_Click(object sender, EventArgs e)
        {
            if (comboBoxRdb.SelectedIndex == -1) return;

            var list = _globalConfig.RDB.BmsInfos.Where(p => p.WriteEnable).ToList();

            var bmsInfo = (BmsInfo)list[comboBoxRdb.SelectedIndex].Clone();

            bmsInfo.Value = textBoxRdb.Text;

            var debugConfig = (DebugConfig)(_globalConfig.RDB.Clone());

            debugConfig.RegisterAddress = (ushort)bmsInfo.StartByte;

            bmsInfo.StartByte = 0;

            debugConfig.DataNumber = (ushort)bmsInfo.ByteLength;

            debugConfig.BmsInfos = new List<BmsInfo>() { bmsInfo };

            TriggerDebug(new MessageHelper(sender, e, (s, args) =>
            {
                try
                {
                    ReadDebugeHandler(debugConfig);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                Invoke(new Action(() =>
                {
                    new Action<object>(FormHelper.ButtonGreen1second).BeginInvoke(sender, null, null);
                    textBoxRdb.Text = bmsInfo.Value;

                }));
            }));
        }

        private GlobalConfig _globalConfig;

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            panel1.Controls.Add(new UcReadWriteBox() { WriteHandler = WriteAfe, ReadHandler = ReadAfe, Dock = DockStyle.Top });
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            panel1.Controls.RemoveAt(panel1.Controls.Count - 1);
        }

    }
}
