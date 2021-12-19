using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class LanguageDataHolder : ScriptableObject, ISerializationCallbackReceiver
{
    [System.Serializable]
    public class LanguageDataItem
    {
        [SerializeField]
        public List<string> langData;

    }
    public Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
    [SerializeField]
    public List<string> keyList = new List<string>();
    [SerializeField]
    public List<LanguageDataItem> valueList = new List<LanguageDataItem>();

    public void OnBeforeSerialize()
    {
        keyList.Clear();
        valueList.Clear();

        foreach (var pair in dict)
        {
            keyList.Add(pair.Key);
            LanguageDataItem item = new LanguageDataItem();
            item.langData = pair.Value;
            valueList.Add(item);
        }
    }

    public void OnAfterDeserialize()
    {
        dict.Clear();

        for (int i = 0; i < keyList.Count; ++i)
        {
            dict[keyList[i]] = valueList[i].langData;
        }
    }
}

