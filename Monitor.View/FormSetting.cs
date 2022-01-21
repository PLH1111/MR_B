using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.Common;

namespace Monitor.View
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }

        public FormSetting(CommConfig communicationCinfig)
        {
            this.communicationCinfig = communicationCinfig;

            InitializeComponent();

            comboBox1.Items.AddRange(SerialPort.GetPortNames());

            comboBox1.Text = communicationCinfig.ComPort;

            comboBox2.Text = communicationCinfig.Baudrate.ToString();
        }

        private CommConfig communicationCinfig;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }

        private void FormSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            communicationCinfig.ComPort = (string)comboBox1.SelectedItem;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            communicationCinfig.Baudrate = int.Parse((string)comboBox2.SelectedItem);
        }
    }
}
