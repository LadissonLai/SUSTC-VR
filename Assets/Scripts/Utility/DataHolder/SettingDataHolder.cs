using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SettingDataHolder : ISerializationCallbackReceiver
{
    public Enums.RunMode runMode { get; set; } = Enums.RunMode.Exercise;
    public int runModeId = 0;
    public Enums.Behaviour behaviour { get; set; } = Enums.Behaviour.Disassembly;
    public int behaviourId = 0;
    public int module { get; set; } = 1;
    public int chapter { get; set; } = 1;
    private Enums.Language lang;
    public int langId = 0;
    public Enums.Language language 
    {
        get {return lang; } 
        set {
            lang = language;
            EntrySetting.SavePlayerData();
            Debug.Log("setting language " + langId + " " + lang + " " + language);
            } 
    }

    public SettingDataHolder()
    {

    }

    public void OnBeforeSerialize()
    {
        runModeId = (int)runMode;
        behaviourId = (int)behaviour;
        langId = (int)language;
    }

    public void OnAfterDeserialize()
    {
        
        runMode = (Enums.RunMode)runModeId;
        behaviour = (Enums.Behaviour)behaviourId;
        language = (Enums.Language)langId;
    }
}