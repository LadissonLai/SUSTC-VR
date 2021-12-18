using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Global : MonoBehaviour
{
    public static Global Instance { get; private set; }

    public bool hasToolInHand;
    public GameObject engineObject;
    private Transform leftHandPoint;
    private Transform rightHandPoint;
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
        hasToolInHand = leftHandPoint != null && leftHandPoint.childCount > 0 
                    || rightHandPoint != null && rightHandPoint.childCount > 0;

        VRTK_InteractableObject interactableObject = engineObject.GetComponent<VRTK_InteractableObject>();
        if(interactableObject != null)
        {
            interactableObject.isGrabbable = !hasToolInHand;
        }
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
