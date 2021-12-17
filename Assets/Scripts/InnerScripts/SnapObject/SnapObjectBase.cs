using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;
using VRTK.GrabAttachMechanics;

public class SnapObjectBase : MonoBehaviour
{
    
    public string prefabName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnSnapped()
    {
        Debug.Log("BaseSnapped");
    }

    public virtual void OnUnsnapped()
    {
        Debug.Log("BaseUnsnapped");
    }
}
