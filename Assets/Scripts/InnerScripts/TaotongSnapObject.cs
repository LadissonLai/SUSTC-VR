using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;

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

        VRTK_ArtificialRotator rotator = transform.gameObject.AddComponent<VRTK_ArtificialRotator>();
        rotator.snapToStep = true;
        // MoveToolHighlightComponents();
        ChangeHierarchy();
        
        GetComponentInChildren<BanshouSnapObject>().enabled = true;
        GetComponentInChildren<BanshouSnapObject>().RecordInitialPosition();
        GetComponentInChildren<TaotongSnapObject>().enabled = false;
    }

    protected void MoveToolHighlightComponents()
    {
        foreach (Component c in transform.Find("ToolHighlight").GetComponents<Component>())
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(c);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(transform.gameObject);
        }
        Destroy(transform.Find("ToolHighlight").gameObject);
    }
    protected void ChangeHierarchy()
    {
        Transform luosiTransform = transform.parent.parent;
        transform.SetParent(luosiTransform.parent);
        // SnapHold snapHoldScript = GetComponentInChildren<SnapHold>();
        // snapHoldScript.StopTransitionCoroutine();
        foreach (Transform t in luosiTransform)
        {
            Destroy(t.gameObject);
        }
        luosiTransform.SetParent(transform);
    }
}
