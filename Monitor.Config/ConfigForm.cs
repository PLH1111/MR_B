using Monitor.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Monitor.Config
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();

            var globalConfig = SerialHelper.ConvertToObject<GlobalConfig>($"{Application.StartupPath}\\Config\\Config.json");

            this.globalConfig = globalConfig;

            Load += OnLoad;

            FormClosing += ConfigForm_FormClosing;
        }

        public ConfigForm(GlobalConfig info)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            globalConfig = info;

            InitializeComponent();

            //利用反射设置DataGridView的双缓冲
            Type dgvType = this.dataGridView1.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this.dataGridView1, true, null);

            Load += OnLoad;

            //propertyGrid1.SelectedObject = globalConfig.Bcu;

            FormClosing += ConfigForm_FormClosing;
        }

        public Action LoadConfig;

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //取消编辑状态
            dataGridView1.EndEdit();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private BindingList<BmsInfo> _bindList;

        private List<BmsInfo> _bcuList;

        private List<BmsInfo> _productList;

        private List<BmsInfo> _rdb;

        private List<BmsInfo> _temp = new List<BmsInfo>();

        private readonly GlobalConfig globalConfig;

        private int indexOfDragRow = -1;

        private int indexOfInsertRow = -1;

        private void OnLoad(object sender, EventArgs e)
        {
            if (globalConfig == null)
            {
                _bcuList = new List<BmsInfo>();
                _bcuList.Add(new BmsInfo());
            }

            if (globalConfig.Bcu.BmsInfos == null)
            {
                _bcuList = new List<BmsInfo>();
                _bcuList.Add(new BmsInfo());
            }
            else
            {
                _bcuList = globalConfig.Bcu.BmsInfos;
            }

            if (globalConfig.ProductInfo.BmsInfos == null)
            {
                globalConfig.ProductInfo.BmsInfos = new List<BmsInfo>();
            }

            if (globalConfig.RDB == null)
            {
                globalConfig.RDB = new DebugConfig();
            }

            if (globalConfig.RDB.BmsInfos == null)
            {
                globalConfig.RDB.BmsInfos = new List<BmsInfo>();
            }

            _productList = globalConfig.ProductInfo.BmsInfos;

            _rdb = globalConfig.RDB.BmsInfos;

            _bindList = new BindingList<BmsInfo>();

            dataGridView1.DataSource = _bindList;

            //样式
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//列宽不自动调整,手工添加列
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;//列宽不自动调整,手工添加列
            dataGridView1.RowHeadersWidth = 12;//行标题宽度固定12
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//不能用鼠标调整列标头宽度
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 247, 252);//奇数行背景色
            dataGridView1.BackgroundColor = Color.White;//控件背景色
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//列标题居中显示
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//单元格内容居中显示
            dataGridView1.RowHeadersVisible = false;

            //行为
            dataGridView1.AutoGenerateColumns = false;//不自动创建列
            dataGridView1.AllowUserToAddRows = false;//不启用添加
            dataGridView1.ReadOnly = false;//启用编辑
            dataGridView1.AllowUserToDeleteRows = true;//启用删除
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//单击单元格选中整行
            dataGridView1.MultiSelect = true;//能多选
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _bindList.Insert(dataGridView1.CurrentCell.RowIndex, new BmsInfo());
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _temp.Clear();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Selected == true)
                {
                    _temp.Add(_bindList[i]);
                }
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = dataGridView1.CurrentCell.RowIndex;

            for (int i = 0; i < _temp.Count; i++)
            {
                _bindList.Insert(i + index, (BmsInfo)_temp[i].Clone());
            }

            for (int i = 0; i < _temp.Count; i++)
            {
                dataGridView1.Rows[i + index].Selected = true;
            }

            dataGridView1.Rows[_temp.Count + index].Selected = false;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            var count = dataGridView1.Rows.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                if (dataGridView1.Rows[i].Selected == true)
                {
                    _bindList.RemoveAt(i);
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

            }
            else
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dataGridView1.Rows[e.RowIndex].Selected == false)
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dataGridView1.SelectedRows.Count == 1)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            _bindList.Insert(dataGridView1.CurrentCell.RowIndex, new BmsInfo());
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            _bindList.Add(new BmsInfo());
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            deleteToolStripMenuItem_Click(null, null);
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
           e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            Point p = dataGridView1.PointToClient(new Point(e.X, e.Y));
            string strRow = (string)e.Data.GetData(DataFormats.StringFormat);
            BmsInfo aimAnimal = SerialHelper.ConvertToObject<BmsInfo>(strRow);
            BmsInfo tempModel = (BmsInfo)aimAnimal.Clone();
      
            var hitTest = dataGridView1.HitTest(p.X, p.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == indexOfDragRow + 1)
                return;

            indexOfInsertRow = hitTest.RowIndex;
            if (e.Effect.Equals(DragDropEffects.Move))
                _bindList.RemoveAt(indexOfDragRow);

            if (indexOfDragRow < indexOfInsertRow)
                indexOfInsertRow--;
            _bindList.Insert(indexOfInsertRow, tempModel);


            dataGridView1.CurrentCell = dataGridView1.Rows[indexOfInsertRow].Cells[0];
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e);
            deleteToolStripMenuItem_Click(sender,e);
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Xlsm file|*.xlsm|allFile|*.*",
                Multiselect = false,
                CheckFileExists = true
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            List<BmsInfo> list = new List<BmsInfo>();

            ThreadPool.QueueUserWorkItem(state => 
            {
                try
                {
                    var listU32 = ChangeDtToList(ofd.FileName, "BC_U32", 4, 32);
                    var listU16 = ChangeDtToList(ofd.FileName, "BC_U16", 2, 16);
                    var listU08 = ChangeDtToList(ofd.FileName, "BC_U8", 1, 08);
                    var listU01 = ChangeDtToList(ofd.FileName, "BC_U1", 1, 01);

                    list.AddRange(listU32);
                    list.AddRange(listU16);
                    list.AddRange(listU08);
                    list.AddRange(listU01);

                    list = list.OrderBy(p => p.DisplayNo).ToList();

                    Invoke(new Action(() => 
                    { 
                        _bindList.Clear();
                        list.ForEach(p => _bindList.Add(p));

                    }));

                    globalConfig.Bcu.DataNumber = (ushort)((listU01.Max(p => p.StartByte)) + 1);

                    Invoke(new Action(() =>
                    {
                        propertyGrid1.Refresh();
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            });
        }

        private List<BmsInfo> ChangeDtToList(string path, string sheetName, int byteLength, int bitLength)
        {
            var dt = ExcelHelper.ExcelSheetImportToDataTable(path, sheetName);
           
            var list = dt.AsEnumerable().ToList();

            list.RemoveAt(0);

            list.RemoveAt(list.Count - 1);

            return list.Select(x => ToBmsInfo(x, byteLength, bitLength)).ToList();
        }

        private BmsInfo ToBmsInfo(DataRow dr, int byteLength, int bitLength)
        {
            var regex   = new Regex("_t");//以_ID_分割
            var list    = regex.Split(dr["DataType"].ToString());

            BmsInfo bmsInfo     = new BmsInfo();
            bmsInfo.Name        = ChangeName(dr["Register name"].ToString());
            bmsInfo.Comment     = dr["Comment"].ToString();
            bmsInfo.Unit        = dr["Unit"].ToString();
            bmsInfo.StartByte   = Convert.ToUInt16(dr["Byte position"].ToString());
            bmsInfo.StartBit    = Convert.ToUInt16(dr["Bit position"]);
            bmsInfo.Factor      = Convert.ToDouble(dr["Coefficient"]);
            bmsInfo.Enable      = Convert.ToUInt16(dr["Display"]) == 1;
            bmsInfo.Log         = Convert.ToUInt16(dr["Log"]) == 1;
            bmsInfo.Decimal     = -(int)Math.Log10(bmsInfo.Factor);
            bmsInfo.BitLength   = bitLength;
            bmsInfo.ByteLength  = byteLength;
            bmsInfo.DataType    = list[0].ToUpper();
            bmsInfo.DisplayNo   = Convert.ToUInt16(dr["DisplayNo"]) == 0 ? 10000 : Convert.ToUInt16(dr["DisplayNo"]);
            bmsInfo.DownLimit   = ChangeType(dr["DownLimit"]);
            bmsInfo.UpLimit     = ChangeType(dr["UpLimit"]);
            return bmsInfo;
        }

        private float ChangeType(object obj)
        {
            if(float.TryParse(obj.ToString(), out float value))
            {
                return value;
            }
            else
            {
                return 0f;
            }
        }
        private string ChangeName(string name)
        {
            var regex   = new Regex("_ID_");//以_ID_分割
            var list    = regex.Split(name);
            var sb      = new StringBuilder();

            if(list.Length > 1)
            {
                if (list[1].Length > 2)
                {
                    foreach (var item in list[1].Split('_'))
                    {
                        if (string.IsNullOrWhiteSpace(item)) continue;
                      
                        var str = item.Substring(1, item.Length - 1);

                        sb.Append(item[0]);
                        sb.Append(str.ToLower());
                    }
                }
            }

            return sb.ToString();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //TreeView treeView = sender as TreeView;

            if (e.Node.Text == "Bcu")
            {
                _bindList = new BindingList<BmsInfo>(_bcuList);

                dataGridView1.DataSource = _bindList;

                //_bindList = new BindingList<BmsInfo>(_bcuList);

                //dataGridView1.DataSource = _bindList;

                propertyGrid1.SelectedObject = globalConfig.Bcu;
            }

            if (e.Node.Text == "ProductInfo")
            {
                _bindList = new BindingList<BmsInfo>(_productList);

                dataGridView1.DataSource = _bindList;

                propertyGrid1.SelectedObject = globalConfig.ProductInfo;
            }

            if (e.Node.Text == "RDB")
            {
                _bindList = new BindingList<BmsInfo>(_rdb);

                dataGridView1.DataSource = _bindList;

                propertyGrid1.SelectedObject = globalConfig.RDB;
            }

            dataGridView1.Refresh();

        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadConfig();
        }
    }
}
