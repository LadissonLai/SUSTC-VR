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
            ContentText.text = string.Format("{0}: {3}\n{1}: {4}\n{2}: {5}",
                            LanguageUtil.Get("run_mode"),
                            LanguageUtil.Get("behaviour"),
                            LanguageUtil.Get("chapter"),
                            LanguageUtil.Get(EntrySetting.Instance.runMode.ToString()),
                            LanguageUtil.Get(EntrySetting.Instance.behaviour.ToString()),
                            LanguageUtil.Get(CommonUtil.GenChapterIndex(EntrySetting.Instance.module, EntrySetting.Instance.chapter).ToString()));

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
