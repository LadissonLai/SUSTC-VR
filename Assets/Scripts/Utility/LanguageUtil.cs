using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class LanguageUtil : MonoBehaviour
{
    private static string excelsFolderPath = null;
    // private static readonly string assetPath = "Assets/Resources/Release/";
    private static string ExcelName = "TextConfig.xlsx";  
    private static Dictionary<string ,LanguageItem> dict = null;
    void Awake()
    {
        excelsFolderPath = Application.dataPath + "/Data/";
        CreatItemArrayWithExcel(excelsFolderPath + ExcelName);
        foreach (var item in dict)
        {
            Debug.Log(item.Key + "--" + item.Value);
            foreach (var innerItem in item.Value.languageData)
            {
                Debug.Log(innerItem);
            }
        }
    }

    private static void CreatItemArrayWithExcel(string filePath)
    {
        //行与列
        int columnNum = 0, rowNum = 0;
        DataRowCollection collection = ExcelUtil.ReadExcel(filePath, 0, ref columnNum, ref rowNum);//获得行与列的值
        //从第二行开始才是有效数据
        dict = new Dictionary<string, LanguageItem>();
        for (int i = 1; i < rowNum; i++)
        {
            LanguageItem item = new LanguageItem();
            dict[collection[i][0].ToString()] = item;
            for(int j = 1; j < columnNum; j++){
                item.languageData.Add(collection[i][j].ToString());
            }
        }
    }


    public static string Get(string key)
    {
        if(dict != null && dict[key] != null && dict[key].languageData != null)
        {
            return dict[key].languageData[(int)Setting.Instance.language];
        }
        return null;
    }
}

class LanguageItem
{
    public List<string> languageData = new List<string>();
}