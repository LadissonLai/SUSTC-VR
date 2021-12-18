using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InnerMenuManager : MonoBehaviour
{
    public void onClickSetting() {

    }
    public void OnClickReturnMain() {
        SceneManager.LoadScene(0);
    }
}
