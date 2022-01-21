using Monitor.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Monitor.View
{
    public partial class UcGrid : UserControl
    {
        public UcGrid()
        {
            //设置窗体的双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            InitializeComponent();

            //利用反射设置DataGridView的双缓冲
            Type dgvType = this.dataGridView1.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this.dataGridView1, true, null);

            //样式
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.RowHeadersWidth = 12;//行标题宽度固定12
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//不能用鼠标调整列标头宽度
            //dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 247, 252);//奇数行背景色
            dataGridView1.BackgroundColor = Color.White;//控件背景色
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//列标题居中显示
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//单元格内容居中显示
            dataGridView1.RowHeadersVisible = false;

            //行为
            //dataGridView1.AutoGenerateColumns = false;//不自动创建列
            dataGridView1.AllowUserToAddRows = false;//不启用添加
            dataGridView1.ReadOnly = false;//启用编辑
            dataGridView1.AllowUserToDeleteRows = false;//不启用删除
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;//单击单元格选中整行
            dataGridView1.MultiSelect = false;//不能多选

            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

            Load += UcGrid_Load;

        }

        //private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        //{
            
        //    ScrollViewer scrollviewer = sender as ScrollViewer;
        //    if (e.Delta > 0)
        //        scrollviewer.LineLeft();
        //    else
        //        scrollviewer.LineRight();
        //    e.Handled = true;
        //}

        private void UcGrid_Load(object sender, EventArgs e)
        {
            UcGrid_PanelConfig();
        }

        private int _editRowIndex;
        private int _editColIndex;

        private string _name;

        private List<IDisplay> _data = new List<IDisplay>();

        private object _obj = new object();

        public Action<string, string> WriteData;

        public void Init(List<IDisplay> data)
        {
            _data = data;

            ResizeEnd();
        }

        #region 鼠标滚动
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        static extern IntPtr WindowFromPoint(Point pt);

        private void dgv_MouseEnter(object sender, EventArgs e)
        {
            this.MouseWheel += dgv_MouseWheel;
        }

        public void dgv_MouseWheel(object sender, MouseEventArgs e)
        {
            var dgv = dataGridView1;
            var slideSpeed = 3;
            Point p = PointToScreen(e.Location);

            if ((WindowFromPoint(p)) == dgv.Handle)//鼠标指针在框内
            {
                int scrollOffset = dgv.HorizontalScrollingOffset;
                if (e.Delta > 0)// 向前滑动
                {
                    scrollOffset = (scrollOffset > slideSpeed) ? (scrollOffset - slideSpeed) : 0;
                }
                else// 向后滑动
                {
                    scrollOffset = (scrollOffset + slideSpeed < dgv.Width) ? (scrollOffset + slideSpeed) : dgv.Width;
                }

                dgv.HorizontalScrollingOffset = scrollOffset;
            }
        }
        #endregion

        public void Refresh(List<IDisplay> data)
        {
            lock (_obj)
            {
                if (dataGridView1.InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                if (_data.Count <= j * dataGridView1.RowCount + i) continue;//避免数组越界

                                if (i == _editRowIndex && j == _editColIndex) continue;//编辑时不刷新

                                if((isValueEqualZero(data[j * dataGridView1.RowCount + i].UpLimit))
                                && (data[j * dataGridView1.RowCount + i].UpLimit == data[j * dataGridView1.RowCount + i].DownLimit))
                                {
                                    ChangeBackColor(i, 3 * j + 1, Color.White);
                                }
                                else
                                {
                                    double value = Convert.ToDouble(data[j * dataGridView1.RowCount + i].Value);
                                    if(value > data[j * dataGridView1.RowCount + i].UpLimit)
                                    {
                                        ChangeBackColor(i, 3 * j + 1, Color.LightPink);
                                    }
                                    else if (value < data[j * dataGridView1.RowCount + i].DownLimit)
                                    {
                                        ChangeBackColor(i, 3 * j + 1, Color.LightPink);
                                    }
                                    else
                                    {
                                        ChangeBackColor(i, 3 * j + 1, Color.White);
                                    }
                                }

                                if (dataGridView1.Rows[i].Cells[3 * j + 1].Value.ToString() != data[j * dataGridView1.RowCount + i].Value.ToString())
                                {
                                    ChangeCellColor(i, 3 * j + 1, Color.Blue);

                                    dataGridView1.Rows[i].Cells[3 * j + 1].Value = data[j * dataGridView1.RowCount + i].Value;
                                }
                                else
                                {
                                    ChangeCellColor(i, 3 * j + 1, Color.Black);
                                }
                            }
                        }
                        //for (int i = 0; i < data.Count; i++)
                        //{
                        //    if (dataGridView1.Rows[i].Cells["Value"].Value.ToString() != data[i].Value.ToString())
                        //    {
                        //        ChangeCellColor(i, Color.Red);

                        //        dataGridView1.Rows[i].Cells["Value"].Value = data[i].Value;
                        //    }
                        //    else
                        //    {
                        //        ChangeCellColor(i, Color.Black);
                        //    }
                        //}
                    }));
                }
                else
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            if (_data.Count <= j * dataGridView1.RowCount + i) continue;

                            if ((isValueEqualZero(data[j * dataGridView1.RowCount + i].UpLimit))
                            && (data[j * dataGridView1.RowCount + i].UpLimit == data[j * dataGridView1.RowCount + i].DownLimit))
                            {
                                ChangeBackColor(i, 3 * j + 1, Color.White);
                            }
                            else
                            {
                                double value = Convert.ToDouble(data[j * dataGridView1.RowCount + i].Value);
                                if (value > data[j * dataGridView1.RowCount + i].UpLimit)
                                {
                                    ChangeBackColor(i, 3 * j + 1, Color.LightPink);
                                }
                                else if (value < data[j * dataGridView1.RowCount + i].DownLimit)
                                {
                                    ChangeBackColor(i, 3 * j + 1, Color.LightPink);
                                }
                                else
                                {
                                    ChangeBackColor(i, 3 * j + 1, Color.White);
                                }
                            }

                            if (dataGridView1.Rows[i].Cells[3 * j + 1].Value.ToString() != data[j * dataGridView1.RowCount + i].Value.ToString())
                            {
                                ChangeCellColor(i, 3 * j + 1, Color.Blue);

                                dataGridView1.Rows[i].Cells[3 * j + 1].Value = data[j * dataGridView1.RowCount + i].Value;
                            }
                            else
                            { 
                                ChangeCellColor(i, 3 * j + 1, Color.Black);
                            }
                        }
                    }
                    //for (int i = 0; i < data.Count; i++)
                    //{
                    //    if (dataGridView1.Rows[i].Cells["Value"].Value.ToString() != data[i].Value.ToString())
                    //    {
                    //        ChangeCellColor(i, Color.Red);

                    //        dataGridView1.Rows[i].Cells["Value"].Value = data[i].Value;
                    //    }
                    //    else
                    //    {
                    //        ChangeCellColor(i, Color.Black);
                    //    }
                    //}
                }
            }
        }

        public void ResizeEnd()
        {
            var heigh = dataGridView1.Height + 1;

            var rowCount = ((heigh - 1) / dataGridView1.RowTemplate.Height)  - 2;

            var colCount = ((_data.Count - 1) / rowCount) + 1;

            ResizeDataGrid(colCount, rowCount);
        }

        private void ResizeDataGrid(int colCount, int rowCount)
        {
            lock (_obj)
            {
                dataGridView1.Columns.Clear();

                for (int i = 0; i < colCount; i++)
                {
                    dataGridView1.Columns.Add("Name",  "Name");
                    dataGridView1.Columns.Add("Value", "Value");
                    dataGridView1.Columns.Add("Uint",  "Uint");
                }

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (i % 3 == 0)//name
                    {
                        dataGridView1.Columns[i].DefaultCellStyle.BackColor = Color.FromArgb(224, 224, 224);

                        dataGridView1.Columns[i].ReadOnly = true;
                    }
                    if (i % 3 == 2)//unit
                    {
                        dataGridView1.Columns[i].ReadOnly = true;
                    }
                    if (i % 3 == 1)//value
                    {
                        dataGridView1.Columns[i].ReadOnly = false;
                    }

                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                for (int i = 0; i < rowCount; i++)
                {
                    object[] objs = new object[colCount * 4];

                    for (int j = 0; j < colCount; j++)
                    {
                        if (_data.Count <= j * rowCount + i) continue;

                        objs[3 * j + 0] = _data[j * rowCount + i].Name;
                        objs[3 * j + 1] = _data[j * rowCount + i].Value;
                        objs[3 * j + 2] = _data[j * rowCount + i].Unit;
                    }

                    dataGridView1.Rows.Add(objs);

                    for (int k = 0; k < colCount; k++)
                    {
                        if (_data.Count <= k * rowCount + i) continue;

                        dataGridView1.Rows[i].Cells[3 * k + 0].ToolTipText = _data[k * rowCount + i].Comment;
                        dataGridView1.Rows[i].Cells[3 * k + 1].ToolTipText = _data[k * rowCount + i].Comment;
                        dataGridView1.Rows[i].Cells[3 * k + 2].ToolTipText = _data[k * rowCount + i].Comment;
                    }
                }
            }
        }

        private void ChangeCellColor(int rowIndex, int colIndex, Color color)
        {
            dataGridView1.Rows[rowIndex].Cells[colIndex].Style.ForeColor = color;
        }
        private void ChangeBackColor(int rowIndex, int colIndex, Color color)
        {
            dataGridView1.Rows[rowIndex].Cells[colIndex].Style.BackColor = color;
        }

        public string TipsName
        {
            get => _name;
            set
            {
                labelName.Text = value;
                _name = value;
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _editRowIndex = e.RowIndex;

            _editColIndex = e.ColumnIndex;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var name = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString();

            var value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            WriteData(name, value);

            _editRowIndex = -1;
            _editColIndex = -1;
        }

        private void UcGrid_PanelConfig()
        {
#if CUSTOMER
            dataGridView1.ReadOnly = true;
#else
            dataGridView1.ReadOnly = false;
#endif
        }

        private bool isValueEqualZero(float value)
        {
            if((value > -0.000000001) && (value < 0.000000001))
            {
                return true;
            }

            return false;
        }
    }
}
