using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class JiYouPaiFang : StepControllerBase
{
    protected override void Awake()
    {
        base.Awake();
        StepFunctions = new string[]{"Origin", "Banshou", "Success"};
        ConditionNum = new int[]{1, 1};
        
        Debug.Log("ChildAwake");
    }
    
    public void Banshou()
    {
        Debug.Log("NotifyBanshou");
    }

    public virtual void Success() 
    { 
        Debug.Log("NotifySuccess");
        Destroy(GameObject.Find("101Object"));
    }
    
}
