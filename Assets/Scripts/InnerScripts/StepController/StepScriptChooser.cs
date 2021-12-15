using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;

public class StepScriptChooser : MonoBehaviour
{

    void Awake() 
    { 
        string scriptName = FindScriptInExcel();
        CommonUtil.AddComponent(transform.gameObject, scriptName);
    }

    private string FindScriptInExcel()
    {
        string filePath = Application.dataPath + "/Data/ChapterScriptConfig.xlsx";
        int columnNum = 0, rowNum = 0;
        DataRowCollection collection = ExcelUtil.ReadExcel(filePath, 0, ref columnNum, ref rowNum);//获得行与列的值
        int key = 101;
        if(EntrySetting.Instance != null) key = EntrySetting.Instance.module * 100 + EntrySetting.Instance.chapter;
        for(int i = 1; i < rowNum; i++){
            if( string.Equals(collection[i][0].ToString(), key.ToString()) )
            {
                return collection[i][1].ToString();
            }
        }
        return null;
    }

}
