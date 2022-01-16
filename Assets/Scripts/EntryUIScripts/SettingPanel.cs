using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Dropdown LanguageDropDown;
    // Start is called before the first frame update
    void Start()
    {
        string[] languageNames = Enum.GetNames(typeof(Enums.Language));
        LanguageDropDown.options.Clear();
        Dropdown.OptionData tempData;
        foreach (var item in languageNames)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = LanguageUtil.Get(item);
            LanguageDropDown.options.Add(tempData);
        }
        LanguageDropDown.captionText.text = LanguageUtil.Get(languageNames[(int)EntrySetting.Instance.language]);
        LanguageDropDown.onValueChanged.AddListener((value) => {
            Debug.Log("Language: "+value);
            EntrySetting.Instance.language = (Enums.Language)value;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
