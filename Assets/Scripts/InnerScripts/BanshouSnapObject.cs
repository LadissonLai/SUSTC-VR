using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Controllables.ArtificialBased;
using VRTK.GrabAttachMechanics;

public class BanshouSnapObject : SnapObjectBase
{
    public Vector3 initialPosition;
    protected VRTK_ArtificialRotator controlRotator;
    protected VRTK_RotateTransformGrabAttach controlGrabAttach;

    public override void OnSnapped()
    {
        Debug.Log("BanshouSnapped");
        SetParam();
        ManageGrabbableListeners(true);
    }

    public override void OnUnsnapped()
    {
        Debug.Log("BanshouUnsnapped");
        ManageGrabbableListeners(false);
    }

    public void RecordInitialPosition()
    {
        initialPosition = transform.parent.localPosition;
    }
    private void SetParam()
    {
        controlRotator = GetComponentInParent<VRTK_ArtificialRotator>();
        controlGrabAttach = GetComponentInParent<VRTK_RotateTransformGrabAttach>();
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
            transform.parent.localPosition = initialPosition + Vector3.up * (currentValue / 9000.0f);
            // Debug.Log(currentValue.ToString() + "---" + (currentValue / 18000.0f).ToString() + "===" + transform.localPosition.ToString());
        }
    }
}
