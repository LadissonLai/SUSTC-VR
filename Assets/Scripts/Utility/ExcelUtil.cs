using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;
using ExcelDataReader;

public class ExcelUtil
{
    /// <summary>
    /// 读取excel文件内容
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="columnnum">行数</param>
    /// <param name="rownum">列数</param>
    /// <returns></returns>
    public static DataRowCollection ReadExcel(string filePath, int sheetIndex, ref int columnnum, ref int rownum)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet(); 
        //Tables[0] 下标0表示excel文件中第一张表的数据
        columnnum = result.Tables[sheetIndex].Columns.Count;
        rownum = result.Tables[sheetIndex].Rows.Count;
        stream.Close();
        return result.Tables[sheetIndex].Rows; 
    }
}
