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
    public partial class UcTextBox : UserControl
    {
        public UcTextBox()
        {
            InitializeComponent();
        }

        public UcTextBox(BmsInfo bmsInfo)
        {
            InitializeComponent();

            Init(bmsInfo);
        }

        private BmsInfo bmsInfo;

        public void Init(BmsInfo bmsInfo)
        {
            this.bmsInfo = bmsInfo;

            if (textBox1.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    label1.Text = bmsInfo.Name + ": ";

                    textBox1.Text = bmsInfo.Value;
                }));
            }
            else
            {
                label1.Text = bmsInfo.Name + ": ";

                textBox1.Text = bmsInfo.Value;
            }
        }

        public new EventHandler<KeyEventArgs> KeyPress;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            KeyPress(this, e);
        }

        public void GetData()
        {
            bmsInfo.Value = textBox1.Text;
        }

        public new void Focus()
        {
            textBox1.Focus();
        }


    }
}
