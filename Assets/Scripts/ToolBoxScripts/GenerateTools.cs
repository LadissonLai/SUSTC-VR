using VRTK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GenerateTools : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform leftHandPoint;
    private Transform rightHandPoint;
    private Transform curHandPoint;
    public GameObject taotong_019;
    public GameObject jiLunL;
    private void recursive(GameObject parent, bool isLeft) {
        foreach(Transform child in parent.transform) {
            if(child.name == "GrabAttachPoint") {
                if(isLeft) {
                    leftHandPoint = child;
                } else {
                    rightHandPoint = child;
                }
                
            }
            recursive(child.gameObject, isLeft);
        }
    }
    public void DoPointerIn(object sender, DestinationMarkerEventArgs e)
    {
        Debug.Log(e.controllerReference.index);
        if(e.controllerReference.actual == VRTK_DeviceFinder.GetControllerLeftHand()) {
            curHandPoint = leftHandPoint;
        } else {
            curHandPoint = rightHandPoint;
        }
    }
    void Start() {
        recursive(VRTK_DeviceFinder.GetControllerLeftHand(), true);
        recursive(VRTK_DeviceFinder.GetControllerRightHand(), false);
        VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_DestinationMarker>().DestinationMarkerEnter += DoPointerIn;
        VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_DestinationMarker>().DestinationMarkerEnter += DoPointerIn;
    }
    public void onClickTaoTong019() {
        GameObject tool = Instantiate(taotong_019, leftHandPoint.transform) as GameObject;
        tool.transform.localPosition = Vector3.zero;
    }
    public void onClickJiLunL() {
        GameObject tool = Instantiate(jiLunL, rightHandPoint.transform) as GameObject;
        tool.transform.localPosition = Vector3.zero;
    }
}
