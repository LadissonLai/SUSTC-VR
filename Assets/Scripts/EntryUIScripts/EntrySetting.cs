using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySetting : MonoBehaviour
{
    public enum RunMode {Exercise, Teaching, Exam};
    public enum Behaviour {Assembly, Disassembly};


    public static EntrySetting Instance { get; private set; }
    public RunMode runMode { get; set; } = 0;
    public Behaviour behaviour { get; set; } = 0;
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