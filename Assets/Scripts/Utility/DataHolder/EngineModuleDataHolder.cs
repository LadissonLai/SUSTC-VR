using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EngineModuleDataHolder : ScriptableObject
{
    public List<string> assembly;
    public List<string> disassembly;
}
