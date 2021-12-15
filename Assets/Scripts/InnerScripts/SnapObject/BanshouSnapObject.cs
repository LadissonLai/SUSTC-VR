using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;
using VRTK.GrabAttachMechanics;

public class BanshouSnapObject : SnapObjectBase
{
    public float maxAngle;
    public bool isAssembly;
    protected Vector3 initialPosition;
    protected Transform controlTransform;
    protected VRTK_InteractableObject controlInteract;
    protected VRTK_ArtificialRotator controlRotator;
    protected VRTK_RotateTransformGrabAttach controlGrabAttach;
    
    void Awake()
    {
        RecordInitialPosition();
    }

    public override void OnSnapped()
    {
        Debug.Log("BanshouSnapped");
        ManageGrabbableListeners(true);
    }

    public override void OnUnsnapped()
    {
        Debug.Log("BanshouUnsnapped");
        ManageGrabbableListeners(false);
        CheckAngle();
    }

    public void RecordInitialPosition()
    {
        SetParam();
        if(isAssembly)
        {
            controlRotator.angleLimits = new Limits2D(-maxAngle, 0f);
            // controlRotator.stepValueRange = new Limits2D(0f, maxAngle);
        }
        else
        {
            controlRotator.angleLimits = new Limits2D(0f, maxAngle);
            // controlRotator.stepValueRange = new Limits2D(-maxAngle, 0f);
        }
        controlRotator.stepValueRange = new Limits2D(0f, maxAngle);
        // controlRotator.stepSize = 1f;
        // controlRotator.snapToStep = true;
        controlInteract.isGrabbable = true;
        // Debug.Log("kkkkkkkkkkkkkkkkkkk");
    }
    private void SetParam()
    {
        controlTransform = GameObject.Find("101Object").transform.GetChild(0);
        initialPosition = controlTransform.localPosition;
        controlRotator = controlTransform.GetComponentInChildren<VRTK_ArtificialRotator>();
        controlGrabAttach = controlTransform.GetComponentInChildren<VRTK_RotateTransformGrabAttach>();
        controlInteract = controlTransform.GetComponentInChildren<VRTK_InteractableObject>();
    }

    protected virtual void ManageGrabbableListeners(bool state)
    {
        if (controlGrabAttach != null)
        {
            if (state)
            {
                controlGrabAttach.AngleChanged += GrabMechanicAngleChanged;
            }
            else
            {
                controlGrabAttach.AngleChanged -= GrabMechanicAngleChanged;
            }
        }
    }
    protected virtual void GrabMechanicAngleChanged(object sender, RotateTransformGrabAttachEventArgs e)
    {
        if (controlGrabAttach != null)
        {
            float currentValue = controlRotator.GetValue();
            controlTransform.localPosition = initialPosition - Vector3.up * (currentValue / 9000.0f);
            // Debug.Log(currentValue.ToString() + "---" + (currentValue / 18000.0f).ToString() + "===" + transform.localPosition.ToString());
        }
    }

    protected virtual void CheckAngle()
    {
        float currentValue = controlRotator.GetValue();
        float currentValueAbs = currentValue >= 0 ? currentValue : -currentValue;
        if(maxAngle - currentValueAbs <= 5f)
        {
            CommonUtil.NotifyStepController();
        }
    }
}
