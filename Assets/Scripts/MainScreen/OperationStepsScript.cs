using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using Doozy.Engine;
using Framework;
using Fxb.CPTTS;

namespace Fxb.CMSVR
{
    public class OperationStepsScript : MonoBehaviour
    {
        public GameObject Step;
        public GameObject Content;
        private List<GameObject> steps;
        private int doneCount = 0;
        private Color DarkGray = Color.grey;
        private Color Black = Color.black;
        private bool hasFirstUndo = false;
        TaskCsvConfig taskCfg;
        ITaskModel taskModel;
        // Start is called before the first frame update
        void Awake() {
            Message.AddListener<PrepareTaskMessage>(OnprepareTaskMessage);
            Message.AddListener<RefreshRecordItemStateMessage>(Onrefresh);
        }
        
        void OnDestroy(){
            
            Message.RemoveListener<PrepareTaskMessage>(OnprepareTaskMessage);
            Message.RemoveListener<RefreshRecordItemStateMessage>(Onrefresh);
        }
        void Start() {
            steps = new List<GameObject>();
        }

        void OnprepareTaskMessage(PrepareTaskMessage msg) {
            taskModel = World.Get<ITaskModel>();
            loadScreen();
        }
        void Onrefresh(RefreshRecordItemStateMessage msg) {
            taskModel = World.Get<ITaskModel>();
            loadScreen();
        }

        private void checkStepState(GameObject step, bool done) {
            var imgColor = step.GetComponentInChildren<Image>().color;
            if(done) {
                doneCount++;
                // 颜色设置为灰色，显示√
                step.GetComponentInChildren<Text>().color = DarkGray;
                step.GetComponentInChildren<Image>().color = new Color(imgColor.r, imgColor.g, imgColor.b,255);
            } else {
                // 颜色设置为黑色，隐藏√
                step.GetComponentInChildren<Text>().color = Black;
                step.GetComponentInChildren<Image>().color = new Color(imgColor.r, imgColor.g, imgColor.b,0);
            }

        }

        // 暴露出的接口

        // 加载指定页面
        public void loadScreen() {
            // initialize
            foreach(var item in steps) {
                Destroy(item);
            }
            hasFirstUndo = false;
            doneCount = 0;
            var curPage = taskModel.GetData()[0];
            var stepGroups = curPage.stepGroups;
            // load new steps
            for (int i = 0; i < stepGroups.Count; i++) {
                GameObject tmpStep = Instantiate(Step, Content.transform) as GameObject;
                tmpStep.GetComponentInChildren<Text>().text = (i+1).ToString() + ". " + taskModel.GetStepGroupDescription(stepGroups[i].id);
                if(!hasFirstUndo && !taskModel.CheckStepGroupCompleted(stepGroups[i].id)) {
                    tmpStep.GetComponentInChildren<Text>().fontSize = (int)((double)tmpStep.GetComponentInChildren<Text>().fontSize * 1.5);
                    hasFirstUndo = true;
                }
                checkStepState(tmpStep, taskModel.CheckStepGroupCompleted(stepGroups[i].id));
                tmpStep.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30 - (50) * i);
                steps.Add(tmpStep);
            }

            Content.GetComponent<RectTransform>().sizeDelta = new Vector2(Content.GetComponent<RectTransform>().sizeDelta.x, 50 * stepGroups.Count);
            foreach(var item in GetComponentsInChildren<Text>()) {
                if(item.name == "Title") {
                    item.text = curPage.taskTitle;
                }
                if(item.name == "Description") {
                    item.text = "操作步骤";
                }
                if(item.name == "DoneCount") {
                    item.text = doneCount.ToString() + "/" + stepGroups.Count;
                }
            }
        }
    }
}