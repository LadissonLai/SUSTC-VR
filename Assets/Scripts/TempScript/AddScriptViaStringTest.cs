using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AddScriptViaStringTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Type type = Type.GetType("SnapObjectBase");
        transform.gameObject.AddComponent(type);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
