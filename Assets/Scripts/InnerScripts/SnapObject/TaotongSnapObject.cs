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
        DestroyImmediate(GetComponent<VRTK_InteractableObject>());
        // DestroyImmediate(GetComponent<TaotongSnapObject>());

        ChangeHierarchy();
        
        GetComponentInChildren<TaotongSnapObject>().enabled = false;

        CommonUtil.NotifyStepController();
    }

    protected void ChangeHierarchy()
    {
        Transform luosiTransform = GameObject.Find("101Object").transform.GetChild(0);
        GameObject gameObject = new GameObject("Container");
        gameObject.transform.SetParent(luosiTransform.parent);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        transform.SetParent(gameObject.transform);
        VRTK_ArtificialRotator rotator = gameObject.AddComponent<VRTK_ArtificialRotator>();
        rotator.snapToStep = true;
        gameObject.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
        // SnapHold snapHoldScript = GetComponentInChildren<SnapHold>();
        // snapHoldScript.StopTransitionCoroutine();
        foreach (Transform t in luosiTransform)
        {
            Destroy(t.gameObject);
        }
        luosiTransform.SetParent(transform);
    }
}
