using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Fxb.CMSVR
{
    public class TaskItemView : MonoBehaviour
    {
        public TextMeshProUGUI title;

        public Button titleBtn;

        // public Transform progressMark;

        // 没给到level的切图，暂时不做
        // public Image levelIcon;

        // TextMeshProUGUI levelTitle;

        public event Action<string> OnTitleBtnClicked;

        TaskCsvConfig taskCfg;

        SpriteAtlas spriteAtlas;

        string id;

        // float middleColor = 86 / 255f;

        // Start is called before the first frame update
        void Start()
        {
            titleBtn.onClick.AddListener(TitleBtnClicked);
        }

        public void Refresh(string taskID, SpriteAtlas atlas = null)
        {
            // levelTitle = levelTitle ?? levelIcon.GetComponentInChildren<TextMeshProUGUI>();

            taskCfg = taskCfg ?? World.Get<TaskCsvConfig>();

            var data = taskCfg.FindRowDatas(taskID);

            if (atlas != null)
                spriteAtlas = atlas;

            // titleBtn.image.sprite = spriteAtlas.GetSprite(data.Icon);

            title.text = data.Title;

            var taskModel = World.Get<ITaskModel>();

            // if (taskModel == null || taskModel.IsSubmitAllTask)
            //     progressMark.gameObject.SetActive(false);
            // else
            //     progressMark.gameObject.SetActive(taskModel.GetData()[0].taskID == taskID);

            // LoadLevelIcon(data.Level);

            id = taskID;
        }

        void TitleBtnClicked()
        {
            OnTitleBtnClicked?.Invoke(id);
        }

        // void LoadLevelIcon(float level)
        // {
        //     string iconName = null;

        //     switch (level)
        //     {
        //         case 2:
        //             iconName = "icon_label_senior";

        //             levelTitle.text = "高级";

        //             levelTitle.color = Color.white;

        //             break;

        //         case 1:
        //             iconName = "icon_label_middle";

        //             levelTitle.text = "中级";

        //             levelTitle.color = new Color(middleColor, middleColor, middleColor, 1);

        //             break;

        //         case 0:
        //             iconName = "icon_label_primary";

        //             levelTitle.text = "初级";

        //             levelTitle.color = Color.white;

        //             break;

        //         default:
        //             Debug.LogError($"{level}- 无效的任务等级");

        //             return;
        //     }

        //     levelIcon.sprite = spriteAtlas.GetSprite(iconName);
        // }
    }

}