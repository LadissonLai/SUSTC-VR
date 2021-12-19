using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EngineModuleDataHolder : ScriptableObject
{
    [SerializeField]
    public List<string> assembly;
    [SerializeField]
    public List<string> disassembly;
}
