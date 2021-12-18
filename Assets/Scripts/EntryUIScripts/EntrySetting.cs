using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySetting : MonoBehaviour
{
    public static EntrySetting Instance { get; private set; }
    public Enums.RunMode runMode { get; set; } = Enums.RunMode.Exercise;
    public Enums.Behaviour behaviour { get; set; } = Enums.Behaviour.Disassembly;
    public int module { get; set; } = 1;
    public int chapter { get; set; } = 1;

    public Enums.Language language {get; set; } = Enums.Language.Chinese;
 
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}