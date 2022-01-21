using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.OpenXml4Net.OPC;
using System.Data;
using System.IO;

namespace Monitor.Common
{
    public class ExcelHelper
    {
        /// <summary>
        /// Excel某sheet中内容导入到DataTable中
        /// 区分xsl和xslx分别处理
        /// </summary>
        /// <param name="filePath">Excel文件路径,含文件全名</param>
        /// <param name="sheetName">此Excel中sheet名</param>
        /// <returns></returns>
        public static DataTable ExcelSheetImportToDataTable(string filePath, string sheetName)
        //public static DataTable ExcelImportToDataTable(string filePath)
        {
            DataTable dt = new DataTable();
            //.xlsx  //.xlsm
            #region .xlsx文件处理:XSSFWorkbook
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook wb = WorkbookFactory.Create(file);
                    ISheet sheet = wb./*GetSheetAt(0).*/GetSheet(sheetName);
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    IRow headerRow = sheet.GetRow(0);
                    //一行最后一个方格的编号 即总的列数 
                    for (int j = 0; j < sheet.GetRow(0).LastCellNum; j++)
                    {
                        //SET EVERY COLUMN NAME
                        ICell cell = headerRow.GetCell(j);
                        if (cell != null && cell.ToString() != "")
                        {
                            dt.Columns.Add(cell.ToString());
                        }
                        else { continue; }

                    }
                    int colCount = dt.Columns.Count;

                    while (rows.MoveNext())
                    {
                        //IRow row = (XSSFRow)sheet.GetRow(j);
                        IRow row = (IRow)rows.Current;
                        DataRow dr = dt.NewRow();
                        bool addDR = false;
                        if (row.RowNum == 0) continue;//The firt row is title,no need import
                        for (int i = 0; i < colCount; i++)
                        {
                            //cell count>column count,then break //每条记录的单元格数量不能大于表格栏位数量 20140213　　　　　　　　　　　　　　　　　
                            //cell count>column count,then break //每条记录的单元格数量不能大于DataTable的title
                            if (i >= colCount) { break; }

                            ICell cell = row.GetCell(i);

                            if (cell == null) break;

                            if ((i == 0) && (string.IsNullOrEmpty(cell.ToString()) == true))//每行第一个cell为空,break
                            {
                                break;
                            }
                            if (cell != null)
                            {
                                object o = cell;
                                //读取Excel格式，根据格式读取数据类型
                                switch (cell.CellType)
                                {
                                    case CellType.Blank: //空数据类型处理
                                        o = "";
                                        break;
                                    case CellType.String: //字符串类型
                                        o = cell.StringCellValue;
                                        break;
                                    case CellType.Numeric: //数字类型  
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        { o = cell.DateCellValue; }
                                        else
                                        {
                                            o = cell.ToString();
                                        }
                                        break;
                                    case CellType.Formula:
                                        //HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(hssfworkbook);
                                        IFormulaEvaluator e = WorkbookFactory.CreateFormulaEvaluator(wb);
                                        //o = e.Evaluate(cell).StringValue;
                                        o = e.Evaluate(cell).NumberValue;
                                        break;
                                    default:
                                        o = "";
                                        break;
                                }
                                dr[i] = Convert.ToString(o);//row.GetCell(j).StringCellValue;
                                addDR = true;
                            }
                        }
                        if (addDR)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion
            return dt;
        }
    }
}

