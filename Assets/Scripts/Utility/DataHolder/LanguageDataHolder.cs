using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class LanguageDataHolder : ScriptableObject
{
    public Dictionary<string, List<string>> dict;
}