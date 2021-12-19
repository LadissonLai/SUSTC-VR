using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Scripting;
using System;

public class CommonUtil : MonoBehaviour
{
    public static GameObject UtilObject = null;
    public static StepControllerBase StepController = null;
    private static InnerPanelManager panelManager = null;
    void Awake()
    {
        CommonUtil.UtilObject = gameObject;
        // GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
    }
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
        // FindUtilObject();
        GetStepController();
        StepController.Notify();
    }

    private static void FindUtilObject()
    {
        if(UtilObject == null)
        {
            UtilObject = GameObject.Find("Utils");
        }
    }
    public static int GenChapterStepIndex(int idx, int step) {
        return idx * 100 + step + 1;
    }
    public static int GenChapterIndex(int module, int chapter) {
        return module * 100 + chapter;
    }

    public static StepControllerBase GetStepController()
    {
        if(StepController == null)
        {
            // FindUtilObject();
            Debug.Log(UtilObject);
            StepController = UtilObject.GetComponent<StepControllerBase>();
            Debug.Log("StepControllerBase " + StepController);
        }
        return StepController;
    }

    public static void UpdateTip(int step)
    {
        int module = EntrySetting.Instance == null ? 1 : EntrySetting.Instance.module;
        int chapter = EntrySetting.Instance == null ? 1 : EntrySetting.Instance.chapter;
        int index = GenChapterStepIndex(GenChapterIndex(module, chapter), step);
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
