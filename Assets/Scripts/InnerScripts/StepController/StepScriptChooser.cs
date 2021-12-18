using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;

public class StepScriptChooser : MonoBehaviour
{
    public Transform tempTransform;
    private static string dataName = "ChapterScriptConfig";  
    private static StepScriptDataHolder holder = null;

    void Awake() 
    { 
        string scriptName = FindScriptInExcel();
        Debug.Log("scriptName " + scriptName);
        CommonUtil.AddComponent(transform.gameObject, scriptName);
    }

    private string FindScriptInExcel()
    {
#if UNITY_EDITOR
        CreateDataAssetWithExcel();
#endif
        holder = Resources.Load<StepScriptDataHolder>(dataName);
        int key = 101;
        if(EntrySetting.Instance != null) key = EntrySetting.Instance.module * 100 + EntrySetting.Instance.chapter;
        if(holder == null || holder.scriptMap == null)
            return null;
        return  holder.scriptMap.ContainsKey(key.ToString()) ? holder.scriptMap[key.ToString()] : holder.scriptMap["101"];
    }

#if UNITY_EDITOR
    private void CreateDataAssetWithExcel()
    {
        string filePath = Application.dataPath + "/Data/ChapterScriptConfig.xlsx";
        int columnNum = 0, rowNum = 0;
        DataRowCollection collection = ExcelUtil.ReadExcel(filePath, 0, ref columnNum, ref rowNum);//获得行与列的值
        StepScriptDataHolder holder = ScriptableObject.CreateInstance<StepScriptDataHolder>();
        holder.scriptMap = new Dictionary<string, string>();
        for(int i = 1; i < rowNum; i++)
        {
            holder.scriptMap.Add(collection[i][0].ToString(), collection[i][1].ToString());
        }
        
        ExcelUtil.CreateAsset(dataName, holder);
    }
#endif
}
