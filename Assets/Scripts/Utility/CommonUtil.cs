using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommonUtil : MonoBehaviour
{
    private static GameObject UtilObject = null;
    private static InnerPanelManager panelManager = null;
    public static Component AddComponent(GameObject gameObject, string componentName)
    {
        Type type = Type.GetType(componentName);
        if(type != null)
        {
            return gameObject.AddComponent(type);
        }
        return null;
    }

    public static void NotifyStepController()
    {
        FindUtilObject();
        UtilObject.GetComponent<StepControllerBase>().Notify();
    }

    private static void FindUtilObject()
    {
        if(UtilObject == null)
        {
            UtilObject = GameObject.Find("Utils");
        }
    }

    public static void UpdateTip(int step)
    {
        int module = EntrySetting.Instance == null ? 1 : EntrySetting.Instance.module;
        int chapter = EntrySetting.Instance == null ? 1 : EntrySetting.Instance.chapter;
        int index = (module * 100 + chapter) * 100 + step;
        UpdateTip(index.ToString());
    }

    public static void UpdateTip(string key)
    {
        if(panelManager == null)
        {
            panelManager = GameObject.Find("PanelManager").GetComponent<InnerPanelManager>();
        }
        panelManager.UpdateTip(key);
    }
}
