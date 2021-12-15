using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class StepControllerBase : MonoBehaviour
{
    protected string[] StepFunctions;

    protected int[] ConditionNum;

    protected int currentStepIndex = 0;

    protected int currentConditionNum = 0;

    protected virtual void Awake() 
    { 
        Debug.Log("BaseAwake");
        CommonUtil.UpdateTip(currentStepIndex);
    }

    public virtual void Notify()
    {
        Debug.Log("Notify");
        
        currentConditionNum += 1;
        if(ConditionNum != null 
            && ConditionNum.Length > currentStepIndex 
            && currentConditionNum >= ConditionNum[currentStepIndex])
        {
            currentConditionNum = 0;
            currentStepIndex += 1;
            {
                NextStep();
            }
        }
    }
    public virtual void NextStep() 
    { 
        Debug.Log("NextStep");
        if(StepFunctions != null)
        {
            // Invoke(StepFunctions[currentStepIndex], 0f);
            
		    MethodInfo vMethodInfo = GetType().GetMethod(StepFunctions[currentStepIndex]);
            vMethodInfo.Invoke(this, null);
        }
        if(currentStepIndex < ConditionNum.Length)
        {
            CommonUtil.UpdateTip(currentStepIndex);
        }
        else
        {
            int runMode = EntrySetting.Instance == null ? 0 : ((int)EntrySetting.Instance.runMode);
            CommonUtil.UpdateTip("Success" + runMode);
        }
    }

    public virtual void Reset() { }

    public virtual void Success() 
    { 
        Debug.Log("NotifySuccess");
    }
}
