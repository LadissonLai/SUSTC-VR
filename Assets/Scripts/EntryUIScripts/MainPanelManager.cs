using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;


public class MainPanelManager : MonoBehaviour
{
    Transform CurrentPanel;
    EngineModuleDataHolder Modules;
    string dataName = "module";
    public GameObject ModuleListItem;

    public Dropdown LanguageDropDown;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Modules = ReadJson("/Data/module.json");
        ExcelUtil.CreateAsset(dataName, Modules);
#endif
        Modules = Resources.Load<EngineModuleDataHolder>(dataName);
        foreach(Transform child in transform){
            HidePanel(child);
        }
        CurrentPanel = transform.Find("EntryCanvas");
        ShowPanel(CurrentPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrintJson(EngineModuleDataHolder EngineModule){
        foreach(string str in EngineModule.assembly){
            Debug.Log(str);
        }
        foreach(string str in EngineModule.disassembly){
            Debug.Log(str);
        }
    }

    public EngineModuleDataHolder ReadJson(string str)
    {
        StreamReader StreamReader = new StreamReader(Application.dataPath + str);
        JsonReader js = new JsonReader(StreamReader);
        return JsonMapper.ToObject<EngineModuleDataHolder>(js);
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
        EntrySetting.Instance.runMode = Enums.RunMode.Exercise;
    }

    public void OnTeachingBtnClicked(){
        SwitchPanel(this.transform.Find("AssAndDisSelectionCanvas"));
        EntrySetting.Instance.runMode = Enums.RunMode.Teaching;
    }

    public void OnExamBtnClicked(){
        //TODO
        Debug.Log("Under Development");
        EntrySetting.Instance.runMode = Enums.RunMode.Exam;
    }

    public void OnSettingBtnClicked(){
        SwitchPanel(this.transform.Find("SettingCanvas"));
        LanguageDropDown.onValueChanged.AddListener((value) => {
            Debug.Log("Language: "+value);
            EntrySetting.Instance.language = (Enums.Language)value;
        });
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
            
            string pattern = @"\[(\d+)\-(\d+)\]";
            int module = int.Parse(Regex.Match(str, pattern).Result("$1"));
            int chapter = int.Parse(Regex.Match(str, pattern).Result("$2"));

            int key = CommonUtil.GenChapterIndex(module, chapter);
            txt.text = LanguageUtil.Get(key.ToString());;

            ButtonInst.GetComponent<Button>().onClick.AddListener( 
                () => { 
                    EntrySetting.Instance.module = module;
                    EntrySetting.Instance.chapter = chapter;
                    Debug.Log("module:" + module + " " + "chapter:" + chapter);
                    // EntrySetting.Instance.module = ButtonInst.transform.GetSiblingIndex(); 
                    SceneManager.LoadScene(1);
                    } );
        }
    }

    public void OnAssemblyBtnClicked(){
        SwitchPanel(this.transform.Find("ModuleSelectionCanvas"));
        SetModuleListContent(Modules.assembly);
        EntrySetting.Instance.behaviour = Enums.Behaviour.Assembly;
    }

    public void OnDisAssemblyBtnClicked(){
        SwitchPanel(this.transform.Find("ModuleSelectionCanvas"));
        SetModuleListContent(Modules.disassembly);
        EntrySetting.Instance.behaviour = Enums.Behaviour.Disassembly;
    }

    public void OnExitBtnClicked(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void OnReturnBtnClicked(){
        if(CurrentPanel == transform.Find("AssAndDisSelectionCanvas") 
            || CurrentPanel == transform.Find("SettingCanvas"))
            SwitchPanel(transform.Find("EntryCanvas"));
        else
            SwitchPanel(transform.Find("AssAndDisSelectionCanvas"));
    }
}
