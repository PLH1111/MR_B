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
    public partial class UcReadWriteBox : UserControl, IIdAndValue
    {
        public UcReadWriteBox()
        {
            InitializeComponent();
        }

        public EventHandler WriteHandler;
        public EventHandler ReadHandler;

        private void BtnAfeWrite_Click(object sender, EventArgs e)
        {
            WriteHandler(this, e);
        }

        private void BtnAfeRead_Click(object sender, EventArgs e)
        {
            ReadHandler(this, e);
        }

        public byte Id 
        {
            get
            {
                var result = false;
                byte data = 0;

                Invoke(new Action(() => { result = byte.TryParse(TbxRegId.Text, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out data); }));

                if (result)
                {
                    return data;
                }
                else
                {
                    MessageBoxHelper.ShowError("Input id foramt error!");
                    return 0;
                }
            }
            set
            {
                //
            }
        }

        public ushort Value 
        { 
            get
            {
                var result = false;
                ushort data = 0;

                Invoke(new Action(() => { ushort.TryParse(TbxRegValue.Text, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out data); }));

                if (result)
                {
                    return data;
                }
                else
                {
                    MessageBoxHelper.ShowError("Input Value foramt error!");
                    return 0;
                }
            }
            set
            {
                Invoke(new Action(() => { TbxRegValue.Text = value.ToString("X4"); }));
            }
        }
    }
}
