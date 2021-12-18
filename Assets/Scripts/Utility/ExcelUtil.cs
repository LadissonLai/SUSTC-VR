using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Windows;
using System.IO;
using System.Data;
using ExcelDataReader;
using UnityEditor;

public class ExcelUtil
{
    private static string assetFolderPath = "Assets/Resources/";
    /// <summary>
    /// 读取excel文件内容
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="sheetIndex">表的页数</param>
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

    public static void CreateAsset(string assetName, Object assetData)
    {
        //确保文件夹存在
        if (!Directory.Exists(assetFolderPath))
        {
            Directory.CreateDirectory(assetFolderPath);
        }

        //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
        string assetPath = string.Format("{0}{1}.asset", assetFolderPath, assetName);
        //生成一个Asset文件
        AssetDatabase.CreateAsset(assetData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
