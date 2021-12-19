using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using VRTK;
using VRTK.Controllables.ArtificialBased;

public class StepControllerBase : MonoBehaviour
{
    protected string[] StepFunctions;

    protected int[] ConditionNum;

    protected int currentStepIndex = 0;

    protected int currentConditionNum = 0;
    
    protected Dictionary<GameObject, Vector3> initPositions;

    protected virtual void Awake() 
    { 
        Debug.Log("BaseAwake");
        // CommonUtil.StepController = this;
        CommonUtil.UpdateTip(currentStepIndex);
        initPositions = new Dictionary<GameObject, Vector3>();
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

    public virtual bool IsInteractable(GameObject gameObject)
    {
        return initPositions != null && initPositions.ContainsKey(gameObject);
    }
    public virtual Vector3 GetInitPosition(GameObject gameObject)
    {
        if(IsInteractable(gameObject))
        {
            return initPositions[gameObject];
        }
        return Vector3.zero;
    } 

    public virtual void ChangeTaotongHierarchy(string operateObjectName)
    {
        GameObject operateObject = GameObject.Find(operateObjectName);
        initPositions.Remove(operateObject);
        Transform luosiTransform = operateObject.transform.GetChild(0);
        GameObject gameObject = new GameObject("Container");
        gameObject.transform.SetParent(luosiTransform.parent);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        Global.Instance.rotatingObject = gameObject;
        
        Transform taotongTransform = luosiTransform.GetChild(luosiTransform.childCount-1).GetChild(1);
        taotongTransform.SetParent(gameObject.transform);
        initPositions.Add(gameObject, gameObject.transform.localPosition);

        VRTK_ArtificialRotator rotator = gameObject.AddComponent<VRTK_ArtificialRotator>();
        rotator.snapToStep = true;
        gameObject.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
        // foreach (Transform t in luosiTransform)
        // {
        //     Destroy(t.gameObject);
        // }
        Destroy(luosiTransform.GetChild(luosiTransform.childCount - 1).gameObject);
        luosiTransform.SetParent(taotongTransform);
    }
}
