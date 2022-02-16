using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using Framework;
using Fxb.CPTTS;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

namespace Fxb.CMSVR
{
    public class TaskMainMenuView : PadViewBase
    {
        public Button[] menuMapBtns;

        public UIView taskView;

        TaskCsvConfig taskCfg;

        public SpriteAtlas spriteAtlas;

        List<TaskCategory> taskCategories;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            taskCfg = World.Get<TaskCsvConfig>();
            GetTaskCategory();
            Debug.Log(taskCategories.ToArray().Length);

            for (var i = 0; i < menuMapBtns.Length; i++)
            {
                var btn = menuMapBtns[i];
                var item  = taskCategories[i];

                btn.onClick.AddListener(() =>
                {
                    EntrySetting.Instance.behaviour = (Enums.Behaviour)i;
                    taskView.Show();
                    taskView.GetComponent<TaskView>().Refresh(item.sysName, item.taskIDs, spriteAtlas);
                });
            }

            doozyView.Show(true);
        }

        void GetTaskCategory()
        {
            taskCategories = new List<TaskCategory>();

            var datas = taskCfg.DataArray;

            foreach (var item in datas)
            {
                var sysName = item.System;

                var category = taskCategories.Find((task) => task.sysName == sysName);

                if (string.IsNullOrEmpty(category.sysName))
                {
                    category.sysName = sysName;

                    category.taskIDs = new List<string>();

                    taskCategories.Add(category);
                }

                category.taskIDs.Add(item.ID);
            }
        }
    }

    [System.Serializable]
    public struct TaskData
    {
        public string sysName;

        public string iconPath;
    }

    // 只有两组, 拆卸和安装
    public struct TaskCategory
    {
        public string sysName;

        public List<string> taskIDs;
    }
}