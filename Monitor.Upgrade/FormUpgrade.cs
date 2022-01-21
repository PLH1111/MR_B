using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using Monitor.Common;
using Monitor.Driver;
using Monitor.Protocol4851._0;

namespace Monitor.Upgrade
{
    public partial class FormUpgrade : Form
    {
        private          bool              _isOpen;
        private          FileHelper        _fileHelper;
        private readonly Upgrade           _upgrade;
        private readonly ITransmitPort     _port;
        private readonly Protocol          _protocol;
        private          UpgradeTimeConfig _timeConfig;
        private          UpgradeConfig     _upgradeConfig;

        public FormUpgrade(UpgradeConfig upgradeConfig) : this()
        {
            _upgradeConfig = upgradeConfig;
        }

        public FormUpgrade()
        {
            InitializeComponent();

            _port     = new Rs485();
            _protocol = new Protocol(_port);

            _upgrade = new Upgrade(_protocol, RefreshText, RefreshMaster, RefreshSlaver, RefreshResult)
            {
                BcuAddress = 0x65,
                BmuAddress = 0,
                Mode       = UpgradeMode.Local
            };

            Load += FormUpgrade_Load;
        }

        private void FormUpgrade_Load(object sender, EventArgs e)
        {
            FormUpgrade_PanelConfig();
            RefreshPort();

            comboBox2.SelectedIndex = 0;

            //HandelFileOInfo(@"C:\Users\Forjanes\Desktop\8010H_AT.bin");

            groupBox3.AllowDrop =  true;
            groupBox3.DragEnter += File_DragEnter;
            groupBox3.DragDrop  += File_DragDrop;

            _timeConfig = SerialHelper.ConvertToObject<UpgradeTimeConfig>(
                Application.StartupPath + "\\Config\\Time.json");

            _protocol.TimeOut            = _timeConfig.CommunicationTimeout;
            _protocol.RetryTimes         = _timeConfig.CommunicationRetryTimes;
            _upgrade.StartTimeout        = _timeConfig.StartTimeout;
            _upgrade.StartTimes          = _timeConfig.StartTimes;
            _upgrade.TranPacketDelayTime = _timeConfig.TranPacketDelayTime;
            _upgrade.CheckProgressTimeout = _timeConfig.CheckProgressTimeout;
            _upgrade.ProcessInterval     = _timeConfig.ProcessInterval;

            if (File.Exists(_upgradeConfig.SelectFilePath))
            {
                textBox1.Text = _upgradeConfig.SelectFilePath;

                HandelFileOInfo(_upgradeConfig.SelectFilePath);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
             
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog{Filter = @"Bin File|*bin" };

            if (fileDialog.ShowDialog() != DialogResult.OK) return;

            HandelFileOInfo(fileDialog.FileName);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (_isOpen)
            {
                _port.Close();
                _isOpen = false;
            }
            else
            {
                _isOpen = SerialPortOpen();
            }

            btnConnect.Text = _isOpen ? "DisConnect" : "Connect";

            toolStripStatusLabel1.Text = _isOpen ? "Connected" : "DisConnected";

            toolStripStatusLabel1.BackColor = _isOpen ? Color.SpringGreen : Color.Transparent;
        }

        private bool SerialPortOpen()
        {
            try
            {
                _port.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == @"Start")
            {
                if (!_isOpen)
                {
                    RefreshText("Please connect port first!");
                    return;
                }

                if (_fileHelper == null)
                {
                    RefreshText("Please select upgrade file first!");
                    return;
                }

                _upgrade.Mode = !chkBoxRemoteUpg.Checked ? UpgradeMode.Local : UpgradeMode.Remote;

                _protocol.Stop = false;

                progressBar1.Value = 0;

                progressBar2.Value = 0;

                btnStart.Text = @"Stop";

                ThreadPool.QueueUserWorkItem(item => { _upgrade.UpgradeThread(); });
            }
            else
            {
                _protocol.Stop = true;

                btnStart.Text = @"Start";
            }

        }

        private void RefreshText(string str)
        {
            if (listBox1.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss.fff}] - -> {str}");

                    LogHelper.Info(str);

                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }));
            }
            else
            {
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss.fff}] - -> {str}");

                LogHelper.Info(str);

                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void RefreshMaster(float value)
        {
            if (progressBar1.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    progressBar1.Value = (int) value;

                    if (listBox1.Items.Count > 0)
                    {
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                        listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss.fff}] - -> Transfer upgrade file Pack {(int)(value * _fileHelper.Packet.Count / 100)}");
                    }
                }));
            }
            else
            {
                progressBar1.Value = (int)value;

                if (listBox1.Items.Count > 0)
                {
                    listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                    listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss.fff}] - -> Transfer upgrade file Pack {(int)(value * _fileHelper.Packet.Count / 100)}");
                }
            }
        }

        private void RefreshSlaver(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    progressBar1.Value = value;
                }));
            }
            else
            {
                progressBar1.Value = value;
            }
        }

        private void RefreshResult(bool value)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    btnStart.Text = @"Start";
                    RefreshText(value ? "Upgrade succeed!" : "Upgrade failed!");
                    listBox1.Items.Add("");
                    if(chkBoxCycleUpg.Checked)
                    {
                        buttonStart_Click(null, null);
                    }
                }));
            }
            else
            {
                btnStart.Text = @"Start";
                RefreshText(value ? "Upgrade succeed!" : "Upgrade failed!");
                listBox1.Items.Add("");
                new Action(() => {
                    if (chkBoxCycleUpg.Checked)
                    {
                        buttonStart_Click(null, null);
                    }
                }).BeginInvoke(null, null);
            }

            //_port.ChangeBaudrate(Convert.ToInt32(comboBox2.SelectedItem));
        }

        private void HandelFileOInfo(string path)
        {
            _upgradeConfig.SelectFilePath = path;

            textBox1.Text = path;

            _fileHelper = new FileHelper(path, 32);

            _upgrade.UpgradeFile = _fileHelper;

            RefreshText($"Select file is {path}");

            RefreshText($"File size is {_fileHelper.ValidSize}");

            RefreshText($"File crc is {_fileHelper.ValidCrc:X8}");

            listBox1.Items.Add("");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            RefreshPort();
        }

        private void RefreshPort()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _port.PortParam = $"{comboBox1.SelectedItem},{comboBox2.SelectedItem}";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _port.PortParam = $"{comboBox1.SelectedItem},{comboBox2.SelectedItem}";
        }

        private void FormUpgrade_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_isOpen) return;

            _port.ClearBuffer();
            _port.Close();
        }

        private void File_DragDrop(object sender, DragEventArgs e)
        {
            string[] filename = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            HandelFileOInfo(Path.GetFullPath(filename[0]));
        }

        private void File_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ?
                DragDropEffects.All : DragDropEffects.None;
        }

        private void chkBoxCycleUpg_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FormUpgrade_PanelConfig()
        {
#if CUSTOMER
            this.chkBoxRemoteUpg.Visible = false;
            this.chkBoxCycleUpg.Visible = false;
#else
            this.chkBoxRemoteUpg.Visible = true;
            this.chkBoxCycleUpg.Visible = true;
#endif

        }
    }
}
