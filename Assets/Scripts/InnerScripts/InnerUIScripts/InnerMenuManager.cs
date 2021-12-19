using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InnerMenuManager : MonoBehaviour
{
    Stack<Transform> st = new Stack<Transform>();
    void Start()
    {
        st.Clear();
        st.Push(transform.Find("MenuCanvas"));
        showPanel(st.Peek());
    }
    private void showPanel(Transform panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 1;
        panel.GetComponent<CanvasGroup>().interactable = true;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    private void hidePanel(Transform panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 0;
        panel.GetComponent<CanvasGroup>().interactable = false;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    private void switchPanel(string canvasName)        
    {
        var panel = transform.Find(canvasName);
        if (panel) {
            hidePanel(st.Peek());
            st.Push(panel);
            showPanel(st.Peek());
        }
    } 
    public void onClickSetting() {
        switchPanel("SettingCanvas");
    }
    public void onClickReturnMain() {
        SceneManager.LoadScene(0);
    }
    public void onClickReturn() {
        if(st.Count > 0)
        {
            hidePanel(st.Pop());
        }
        if(st.Count > 0)
        {
            showPanel(st.Peek());
        }
    }
}
