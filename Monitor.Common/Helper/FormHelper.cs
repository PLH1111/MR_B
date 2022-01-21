using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Monitor.Common
{
    public class FormHelper
    {
        public static void ButtonGreen1second(object sender)
        {
            var button = sender as Button;

            button.Invoke(new Action(() => button.BackColor = Color.Green));

            Thread.Sleep(300);

            button.Invoke(new Action(() => button.BackColor = Color.FromArgb(224, 224, 224)));
        }

        //private void textBoxRegId_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if ((e.KeyChar >= '0' && e.KeyChar <= '9')
        //        || (e.KeyChar >= 'a' && e.KeyChar <= 'f')
        //        || (e.KeyChar >= 'A' && e.KeyChar <= 'F')
        //        || (e.KeyChar == (char)8)//允许输入回退键
        //        || (e.KeyChar == (char)127)
        //        )
        //    {
        //        e.KeyChar = Convert.ToChar(e.KeyChar.ToString().ToUpper());
        //        e.Handled = false;
        //    }
        //    else
        //    {
        //        e.Handled = true;
        //    }
        //}

        //public List<IDisplay> GetData()
        //{
        //    //List<IDisplay> list = SerialHelper.ConvertToObject<List<IDisplay>>(SerialHelper.ConvertToStr(_data));

        //    List<IDisplay> list = _data.Select(p => (IDisplay)(p as BmsInfo).Clone()).ToList();

        //    if (dataGridView1.InvokeRequired)
        //    {
        //        Invoke(new Action(() =>
        //        {
        //            for (int i = 0; i < _data.Count; i++)
        //            {
        //                if (dataGridView1.Rows[i].Cells["Value"] != null)
        //                {
        //                    list[i].Value = dataGridView1.Rows[i].Cells["Value"].Value.ToString();
        //                }
        //            }
        //        }));
        //    }
        //    else
        //    {
        //        for (int i = 0; i < _data.Count; i++)
        //        {
        //            if (dataGridView1.Rows[i].Cells["Value"] != null)
        //            {
        //                list[i].Value = dataGridView1.Rows[i].Cells["Value"].Value.ToString();
        //            }
        //        }
        //    }

        //    return list;
        //}

    }
}
