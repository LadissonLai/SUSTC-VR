using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class EngineModule{
    public List<string> assembly;
    public List<string> disassembly;
}

public class MainPanelManager : MonoBehaviour
{
    Transform CurrentPanel;
    EngineModule Modules;
    public GameObject ModuleListItem;
    // Start is called before the first frame update
    void Start()
    {
        Modules = ReadJson("/Data/module.json");
        CurrentPanel = this.transform.Find("EntryCanvas");
        foreach(Transform child in transform){
            HidePanel(child);
        }
        ShowPanel(CurrentPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrintJson(EngineModule EngineModule){
        foreach(string str in EngineModule.assembly){
            Debug.Log(str);
        }
        foreach(string str in EngineModule.disassembly){
            Debug.Log(str);
        }
    }

    public EngineModule ReadJson(string str)
    {
        StreamReader StreamReader = new StreamReader(Application.dataPath + str);
        JsonReader js = new JsonReader(StreamReader);
        return JsonMapper.ToObject<EngineModule>(js);
    }

    void ShowPanel(Transform transform){
        transform.GetComponent<CanvasGroup>().alpha = 1;
        transform.GetComponent<CanvasGroup>().interactable = true;
        transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    void HidePanel(Transform transform){
        transform.GetComponent<CanvasGroup>().alpha = 0;
        transform.GetComponent<CanvasGroup>().interactable = false;
        transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void SwitchPanel(Transform panel){
        HidePanel(CurrentPanel);
        ShowPanel(panel);
        CurrentPanel = panel;
    }

    public void OnExerciseBtnClicked(){
        SwitchPanel(this.transform.Find("AssAndDisSelectionCanvas"));
        
    }

    public void OnTeachingBtnClicked(){
        SwitchPanel(this.transform.Find("AssAndDisSelectionCanvas"));
    }

    public void OnExamBtnClicked(){
        //TODO
        Debug.Log("Under Development");
    }

    public void OnSettingBtnClicked(){
        //TODO
        Debug.Log("Under Development");
    }

    void SetModuleListContent(List<string> content){
        Transform ModuleListItemContainer = transform.Find("ModuleSelectionCanvas")
                                                .Find("ModuleScrollView")
                                                .Find("Viewport")
                                                .Find("Content");
        for (int i = 0; i < ModuleListItemContainer.childCount; i++) {  
            Destroy (ModuleListItemContainer.GetChild(i).gameObject);  
        }  
        foreach(string str in content){
            GameObject ButtonInst = Instantiate(ModuleListItem, ModuleListItemContainer);
            Text txt = ButtonInst.transform.GetChild(0).GetComponent<Text>();
            txt.text = str;
        }
    }

    public void OnAssemblyBtnClicked(){
        SwitchPanel(this.transform.Find("ModuleSelectionCanvas"));
        SetModuleListContent(Modules.assembly);
    }

    public void OnDisAssemblyBtnClicked(){
        SwitchPanel(this.transform.Find("ModuleSelectionCanvas"));
        SetModuleListContent(Modules.disassembly);
    }

    public void OnExitBtnClicked(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void OnReturnBtnClicked(){
        if(CurrentPanel == transform.Find("AssAndDisSelectionCanvas"))
            SwitchPanel(transform.Find("EntryCanvas"));
        else
            SwitchPanel(transform.Find("AssAndDisSelectionCanvas"));
    }
}
