/*
* @Author: gsd 
* @Date: 2021-11-27 17:08:25 
* @Last Modified by: jadizhang
* @Last Modified time: 2021-11-27 17:31:06
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class Caller : MonoBehaviour
{
    enum State:int //当前是什么Canvas
    {
        isNull = 0,
        isToolBox = 1,
        isMenu = 2
    }
    public VRTK_ControllerEvents leftController;
    public VRTK_ControllerEvents rightController;
    public GameObject toolboxCanvas;
    public GameObject menuCanvas;
    private State state;
    private GameObject currentCanvas;

    protected virtual void OnEnable()
    {
        state = State.isNull;
        RegisterEvents(leftController);
        RegisterEvents(rightController);
    }

    protected virtual void RegisterEvents(VRTK_ControllerEvents events)
    {
        if (events != null)
        {
            events.ButtonTwoPressed += ButtonTwoPressed;
        }
    }

    protected virtual void ButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        DestroyIfNeed();

        state = (State)((int)(state + 1) % Enum.GetNames(typeof(State)).Length);
        CreateCanvas();
    }
    private void DestroyIfNeed() {
        if(currentCanvas) {
            Destroy(currentCanvas);
        }
    }
    private void CreateCanvas() {
        Transform playArea = VRTK_DeviceFinder.PlayAreaTransform();
        Transform headset = VRTK_DeviceFinder.HeadsetTransform();
        if (playArea != null && headset != null) {
            transform.position = new Vector3(headset.position.x, headset.position.y, headset.position.z);
            switch (state) {
                case State.isNull:
                    currentCanvas = null;
                    break;
                case State.isToolBox:
                    currentCanvas = Instantiate(toolboxCanvas, transform) as GameObject;
                    break;
                case State.isMenu:
                    currentCanvas = Instantiate(menuCanvas, transform) as GameObject;
                    break;
                default:
                    break;
            }
            if(currentCanvas) {
                currentCanvas.transform.localPosition = headset.forward * 3.0f;
                currentCanvas.transform.localPosition = new Vector3(currentCanvas.transform.localPosition.x, 0f, currentCanvas.transform.localPosition.z);
                Vector3 targetPosition = headset.position;
                targetPosition.y = playArea.transform.position.y;
                currentCanvas.transform.LookAt(targetPosition);
                currentCanvas.transform.localEulerAngles = new Vector3(0f, currentCanvas.transform.localEulerAngles.y + 180, 0f);
                currentCanvas.SetActive(true);
            }
        }
    }
    
    public void ResetState() {
        state = State.isNull;
    }
}