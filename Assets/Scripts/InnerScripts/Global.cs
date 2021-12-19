using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Global : MonoBehaviour
{
    public static Global Instance { get; private set; }

    public bool hasToolInHand;
    public GameObject engineObject;
    public VRTK_InteractableObject interactableObject;
    public BanshouSnapObject banshouObejct;
    public GameObject rotatingObject;
    private Transform leftHandPoint;
    private Transform rightHandPoint;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        recursive(VRTK_DeviceFinder.GetControllerLeftHand(), true);
        recursive(VRTK_DeviceFinder.GetControllerRightHand(), false);
    }

    void Update()
    {
        CheckEngineGrabbable();
    }

    private void CheckEngineGrabbable()
    {
        if(leftHandPoint == null)
        {
            recursive(VRTK_DeviceFinder.GetControllerLeftHand(), true);
        }
        if(rightHandPoint == null)
        {
            recursive(VRTK_DeviceFinder.GetControllerRightHand(), false);
        }
        // hasToolInHand = leftHandPoint != null && leftHandPoint.childCount > 0 
        //             || rightHandPoint != null && rightHandPoint.childCount > 0;

        interactableObject.isGrabbable = !hasToolInHand;
    }

    private void recursive(GameObject parent, bool isLeft) 
    {
        if(parent == null) return;
        foreach(Transform child in parent.transform) 
        {
            if(child.gameObject.name == "GrabAttachPoint") 
            {
                if(isLeft) 
                {
                    leftHandPoint = child;
                } 
                else 
                {
                    rightHandPoint = child;
                }
                
            }
            recursive(child.gameObject, isLeft);
        }
    }
}
