using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InnerPanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(EntrySetting.Instance != null)
        {
            Text ContentText = GameObject.Find("EntrySettingShow").transform
                                                    .GetChild(0).GetChild(1).GetComponent<Text>();
            ContentText.text = string.Format("RunMode: {0:G}\nBehaviour: {1:G}\nModule: {2:D}",
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
}
