﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySetting : MonoBehaviour
{
    public static EntrySetting Instance { get; private set; }
    public Enums.RunMode runMode { get; set; } = 0;
    public Enums.Behaviour behaviour { get; set; } = 0;
    public int module { get; set; }
 
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