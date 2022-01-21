using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.Common;

namespace Monitor.View
{
    public partial class FormProduct : Form
    {
        public FormProduct()
        {
            InitializeComponent();

            Load += Form_Load;
        }

        public DebugConfig Config { get; set; }

        public Action<DebugConfig> Write;
        public Action<DebugConfig> Read;

        private List<BmsInfo> bmsInfos = new List<BmsInfo>();

        private List<UcTextBox> ucInfos = new List<UcTextBox>();

        public Action<MessageHelper> TiggerMessage;

        private void Form_Load(object sender, EventArgs e)
        {
            bmsInfos = Config.BmsInfos;

            //ucGrid1.Init(bmsInfos.Select(p => p as IDisplay).ToList());

            ucInfos.AddRange(bmsInfos.Select(p => new UcTextBox(p)));

            ucInfos.ForEach(p => p.KeyPress += FormProduct_KeyPress );

            flowLayoutPanel1.Controls.AddRange(ucInfos.ToArray());
        }

        private void FormProduct_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            var index = ucInfos.FindIndex(p => p == (UcTextBox)sender);

            if (index == ucInfos.Count - 1) return;

            ucInfos[index + 1].Focus();
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            TiggerMessage(new MessageHelper(sender, e, (o, ergs) => 
            {
                Read(Config);
#if Old
                ucGrid1.Refresh(Config.BmsInfos.Select(p => p as IDisplay).ToList());
#else
                for (int i = 0; i < bmsInfos.Count; i++)
                {
                    ucInfos[i].Init(bmsInfos[i]);
                }
#endif

                Invoke(new Action(() =>
                {
                    textBox1.AppendText($"{DateTime.Now:HH:mm:ss}->   Read: {string.Join(",", bmsInfos.Select(p => p.Value))}\r\n");
                }));
            }));
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
#if Old
            var data = ucGrid1.GetData();

            for (int i = 0; i < Config.BmsInfos.Count; i++)
            {
                Config.BmsInfos[i].Value = data[i].Value;
            }
#else

            ucInfos.ForEach(p => p.GetData());

#endif
            TiggerMessage(new MessageHelper(sender, e, (o, ergs) => 
            {
                Write(Config);

                Invoke(new Action(() =>
                {
                    textBox1.AppendText($"{DateTime.Now:HH:mm:ss}->  Write: {string.Join(",", bmsInfos.Select(p => p.Value))}\r\n");
                }));
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ucInfos.ForEach(p => p.GetData());
            var source = bmsInfos.Select(p => p.Value).ToList();

            TiggerMessage(new MessageHelper(sender, e, (o, ergs) =>
            {
                Write(Config);

                Invoke(new Action(() =>
                {
                    textBox1.AppendText($"{DateTime.Now:HH:mm:ss}->  Write: \r\n{string.Join("\r\n", bmsInfos.Select(p => p.Value))}\r\n");
                }));

                Read(Config);

                for (int i = 0; i < bmsInfos.Count; i++)
                {
                    ucInfos[i].Init(bmsInfos[i]);
                }

                var destination = bmsInfos.Select(p => p.Value).ToList();

                var result = Enumerable.SequenceEqual(source, destination);

                Invoke(new Action(() =>
                {
                    textBox1.AppendText($"{DateTime.Now:HH:mm:ss}->   Read: \r\n{string.Join("\r\n", bmsInfos.Select(p => p.Value))}\r\n");

                    textBox1.AppendText($"{DateTime.Now:HH:mm:ss}-> Result: {result}\r\n");
                    if(result)
                    {
                        textBox1.AppendText($"====================写入成功==================\r\n\r\n");
                    }
                    else
                    {
                        textBox1.AppendText($"====================写入失败==================\r\n\r\n");
                    }
                }));
            }));
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
    }

   
}
