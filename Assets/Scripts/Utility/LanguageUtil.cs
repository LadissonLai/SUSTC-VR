using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class LanguageUtil : MonoBehaviour
{
    private static string dataName = "TextConfig";  
    private static LanguageDataHolder assetHolder = null;
    private static Dictionary<string, List<string>> dict;
    // private static Dictionary<string, string[]> dict;
    void Awake()
    {
#if UNITY_EDITOR
        CreateItemArrayWithExcel(string.Format("{0}{1}{2}.xlsx"
                                        , Application.dataPath
                                        , "/Data/"
                                        , dataName));
#endif
        assetHolder = Resources.Load<LanguageDataHolder>(dataName);
    }

#if UNITY_EDITOR
    private static void CreateItemArrayWithExcel(string filePath)
    {
        //行与列
        int columnNum = 0, rowNum = 0;
        DataRowCollection collection = ExcelUtil.ReadExcel(filePath, 0, ref columnNum, ref rowNum);//获得行与列的值
        //从第二行开始才是有效数据
        dict = new Dictionary<string, List<string>>();
        for (int i = 1; i < rowNum; i++)
        {
            List<string> list = new List<string>();
            dict[collection[i][0].ToString()] = list;
            for(int j = 1; j < columnNum; j++){
                list.Add(collection[i][j].ToString());
            }
        }

        LanguageDataHolder assetDict = ScriptableObject.CreateInstance<LanguageDataHolder>();
        assetDict.dict = dict;
        
        ExcelUtil.CreateAsset(dataName, assetDict);
    }
#endif

    public static string Get(string key)
    {
        if(assetHolder == null) return null;
        if(assetHolder.dict != null && assetHolder.dict[key] != null)
        {
            int languageIndex = 0;
            if(EntrySetting.Instance != null) languageIndex = (int)EntrySetting.Instance.language;
            return assetHolder.dict[key][languageIndex];
        }
        return null;

        // if(assetHolder == null) return null;
        // if(dict != null && dict[key] != null)
        // {
        //     int languageIndex = 0;
        //     if(EntrySetting.Instance != null) languageIndex = (int)EntrySetting.Instance.language;
        //     return dict[key][languageIndex];
        // }
        // return null;
    }

}