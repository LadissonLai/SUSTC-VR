using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnerPanelManager : MonoBehaviour
{
    public Text tipText;
    public Image tipImage;
    // Start is called before the first frame update
    void Start()
    {
        if(EntrySetting.Instance != null)
        {
            Text ContentText = GameObject.Find("EntrySettingShow").transform
                                                    .GetChild(0).GetChild(1).GetComponent<Text>();
            ContentText.text = string.Format("{0}: {3:G}\n{1}: {4:G}\n{2}: {5:D}",
                            LanguageUtil.Get("run_mode"),
                            LanguageUtil.Get("behaviour"),
                            LanguageUtil.Get("module"),
                            EntrySetting.Instance.runMode,
                            EntrySetting.Instance.behaviour,
                            EntrySetting.Instance.module);

            Debug.Log(EntrySetting.Instance.runMode);
            Debug.Log(EntrySetting.Instance.behaviour);
            Debug.Log(EntrySetting.Instance.module);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateTip(string key)
    {
        tipText.text = LanguageUtil.Get(key);
        //TODO image 
    }
}
