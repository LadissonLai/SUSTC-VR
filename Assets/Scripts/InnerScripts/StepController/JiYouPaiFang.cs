using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class JiYouPaiFang : StepControllerBase
{
    protected override void Awake()
    {
        base.Awake();
        StepFunctions = new string[]{"Origin", "FangYouLuoSai", "LvQingQiBanShou", "LvQingQiChaiXie","Success"};
        ConditionNum = new int[]{1, 1, 1, 1};
        Debug.Log("ChildAwake");
    }
    
    public void FangYouLuoSai()
    {
        Debug.Log("NotifyFangYouLuoSai");
        ChangeTaotongHierarchy("10101Object");
    }

    public void LvQingQiBanShou()
    {
        GameObject.Find("10101Object").SetActive(false);
        Transform outer = GameObject.Find("jiyoulvqingqizong").transform;
        outer.GetChild(1).gameObject.SetActive(false);
        outer.GetChild(3).gameObject.SetActive(true);
    }


    public void LvQingQiChaiXie()
    {
        Debug.Log("NotifyLvQingQiChaiXie");
        ChangeTaotongHierarchy("10102Object");
    }
    public virtual void Success() 
    { 
        GameObject.Find("10102Object").SetActive(false);
        Debug.Log("NotifySuccess");
    }
    
}
