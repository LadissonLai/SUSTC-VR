using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;
using VRTK.GrabAttachMechanics;

public class TaotongSnapObject : SnapObjectBase
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnSnapped()
    {
        Debug.Log("TaotongSnapped");
        
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        
        foreach (Collider collider in GetComponentsInChildren<SphereCollider>())
        {
            collider.enabled = true;
        }
        Destroy(GetComponent<VRTK_InteractableObject>());
        // GetComponent<VRTK_InteractableObject>().isGrabbable = false;
        
        // GetComponent<VRTK_InteractableObject>().enabled = false;
        Global.Instance.hasToolInHand = false;
        CommonUtil.NotifyStepController();
        GetComponentInChildren<TaotongSnapObject>().enabled = false;

    }

    public override void OnUnsnapped()
    {
        Debug.Log("TaotongUnsnapped");
        // Destroy(GetComponent<VRTK_InteractableObject>());
    }
}
