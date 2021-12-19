using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class StepScriptDataHolder : ScriptableObject, ISerializationCallbackReceiver
{
    public Dictionary<string, string> scriptMap = new Dictionary<string, string>();
    
    [SerializeField]
    public List<string> keyList = new List<string>();
    [SerializeField]
    public List<string> valueList = new List<string>();

    public void OnBeforeSerialize()
    {
        keyList.Clear();
        valueList.Clear();

        foreach (var pair in scriptMap)
        {
            keyList.Add(pair.Key);
            valueList.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        scriptMap.Clear();

        for (int i = 0; i < keyList.Count; ++i)
        {
            scriptMap[keyList[i]] = valueList[i];
        }
    }
}