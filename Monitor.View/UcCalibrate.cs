using System;
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
    public partial class UcCalibrate : UserControl, ICalibrateUiInfo
    {
        public UcCalibrate()
        {
            InitializeComponent();

            Input = new List<string>() { "", "" };
            //Output = new List<string>() { "", "" };

            button1.Tag = 0;
            button2.Tag = 1;

            textBoxIn1.Tag = 0;
            textBoxIn2.Tag = 1;

            button1.Click += ButtonClick;
            button2.Click += ButtonClick;

            textBoxIn1.TextChanged += (o, e) => { Input[(int)((TextBox)o).Tag] = ((TextBox)o).Text; };
            textBoxIn2.TextChanged += (o, e) => { Input[(int)((TextBox)o).Tag] = ((TextBox)o).Text; };
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            ClickHandler(this, new EventArgsCalibrate(Convert.ToByte(((Button)sender).Tag)));
        }

        public EventHandler<EventArgsCalibrate> ClickHandler { get; set; }

        public List<string> Input { get; private set; }

        public float[] Result 
        {
            set
            {
                if (value == null) return;
                if (value.Length < 2) return;

                Invoke(new Action(() =>
                {
                    textBoxGain.Text = value[0].ToString();
                    textBoxOffset.Text = value[1].ToString();
                }));
            }
        }

        private string _calibrateName;
        public string CalibrateName 
        {
            get 
            {
                return _calibrateName;
            }
            set 
            {
                if (textBoxOut1.InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        label4.Text = value;
                    }));
                }
                else
                {
                    label4.Text = value;
                }

                _calibrateName = value;
            }
        }

        public byte Channel { get; set; }

        private string _output1;
        public string Output1 
        { 
            set 
            {
                if (textBoxOut1.InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        textBoxOut1.Text = value;
                    }));
                }
                else
                {
                    textBoxOut1.Text = value;
                }

                _output1 = value;
            }
            get
            {
                return _output1;
            }
        }

        private string _output2;

        public string Output2
        {
            set
            {
                if (textBoxOut2.InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        textBoxOut2.Text = value;

                        float[][] data = new float[2][];

                        data[0] = new float[2] { float.Parse(Input[0]), float.Parse(Output1) };
                        data[1] = new float[2] { float.Parse(Input[1]), float.Parse(value) };

                        var kb = UnitityHelper.GetKb(data);

                       Result = kb;
                    }));
                }
                else
                {
                    textBoxOut2.Text = value;
                }
                _output2 = value;
            }
            get { return _output2; }
        }

    }
}
