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
    private Caller caller;
    public GameObject taotong_019;
    public GameObject jiYouLvQingQiBanshou;
    public GameObject jiLunL;
    private void recursive(GameObject parent, bool isLeft) {
        foreach(Transform child in parent.transform) {
            if(child.gameObject.name == "GrabAttachPoint") {
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
        if(e.controllerReference.actual == VRTK_DeviceFinder.GetControllerLeftHand(true)) {
            curHandPoint = leftHandPoint;
        } else {
            curHandPoint = rightHandPoint;
        }
    }
    void Start() {
        recursive(VRTK_DeviceFinder.GetControllerLeftHand(), true);
        recursive(VRTK_DeviceFinder.GetControllerRightHand(), false);
        curHandPoint = rightHandPoint;
        VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_DestinationMarker>().DestinationMarkerEnter += DoPointerIn;
        VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_DestinationMarker>().DestinationMarkerEnter += DoPointerIn;
        caller = transform.parent.GetComponent<Caller>();
    }
    private void generateTool(GameObject prefab) {
        // 销毁手上已经有的工具
        foreach(Transform obj in curHandPoint) {
            Destroy(obj.gameObject);
        }
        GameObject tool = Instantiate(prefab, curHandPoint.transform) as GameObject;
        tool.transform.localPosition = Vector3.zero;
        Global.Instance.hasToolInHand = true;
        caller.ResetState();
        Destroy(gameObject);
    }
    public void onClickTaoTong019() {
        generateTool(taotong_019);
    }
    public void onClickJiLunL() {
        generateTool(jiLunL);
    }
    public void onClickJiYouLvQingQiBanshou() {
        generateTool(jiYouLvQingQiBanshou);
    }
    public void onClickRelease() {
        Debug.Log("enter");
        foreach(Transform obj in curHandPoint) {
            Destroy(obj.gameObject);
        }
        foreach(Transform obj in curHandPoint) {
            Debug.Log(obj.gameObject);
        }
        Global.Instance.hasToolInHand = false;
        caller.ResetState();
        Destroy(gameObject);
    }
}
