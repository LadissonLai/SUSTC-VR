﻿using System.Collections;
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
    public class CompleteOperationStepsScript: MonoBehaviour {
        public GameObject UpBtn;
        public GameObject DownBtn;
        public GameObject Step;
        public GameObject Content;
        private int curPage = 0;
        private int maxPageIndex;
        private List<GameObject> steps;
        TaskCsvConfig taskCfg;
        TaskStepGroupCsvConfig stepCfgs;
        RecordCsvConfig recordCfgs;
        void Awake() {
            Message.AddListener<ShowStepMessage>(OnShow);
        }
        void Start() {
            stepCfgs = World.Get<TaskStepGroupCsvConfig>();
            taskCfg = World.Get<TaskCsvConfig>();
            recordCfgs = World.Get<RecordCsvConfig>();
            maxPageIndex = taskCfg.DataArray.Count;
            steps = new List<GameObject>();
            if(maxPageIndex > 0) {
                loadScreen(0);
            }
            checkInteractable();
        }
        private void OnShow(ShowStepMessage msg) {
            Debug.Log("gsd get message");
        }
        void checkInteractable() {
            if(curPage > 0) {
                UpBtn.GetComponent<Button>().interactable = true;
            } else {
                UpBtn.GetComponent<Button>().interactable = false;
            }
            if(curPage < maxPageIndex - 1) {
                DownBtn.GetComponent<Button>().interactable = true;
            } else {
                DownBtn.GetComponent<Button>().interactable = false;
            }
        }
        public void onClickPageUp() {
            curPage = (curPage + 1 + maxPageIndex) % maxPageIndex;
            loadScreen(curPage);
            checkInteractable();
        }
        public void onClickPageDown() {
            curPage = (curPage - 1 + maxPageIndex) % maxPageIndex;
            loadScreen(curPage);
            checkInteractable();
        }

        // 暴露出的接口

        // 加载指定页面
        public void loadScreen(int index) {
            // initialize
            foreach(var item in steps) {
                Destroy(item);
            }
            steps.Clear();

            // load new steps
            if(index <  0 || index > taskCfg.DataArray.Count - 1) {
                return;
            }
            var curPage = taskCfg.DataArray[index];
            var stepNames = curPage.StepGroupID.Split(',');
            foreach(var stepName in stepNames) {
                var stepIDs = stepCfgs.FindRowDatas(stepName).StepIDs.Split(',');
                foreach(var stepID in stepIDs) {
                    // Debug.Log("gsd " + stepID + " !! " + recordCfgs.FindRowDatas(stepID).Title);
                    GameObject tmpStep = Instantiate(Step, Content.transform) as GameObject;
                    tmpStep.GetComponentInChildren<Text>().text = (steps.Count + 1).ToString() + ". " + recordCfgs.FindRowDatas(stepID).Title;
                    tmpStep.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30 - (50) * steps.Count);
                    steps.Add(tmpStep);
                }
            }

            Content.GetComponent<RectTransform>().sizeDelta = new Vector2(Content.GetComponent<RectTransform>().sizeDelta.x, 50 * stepNames.Length);
            foreach(var item in GetComponentsInChildren<Text>()) {
                if(item.name == "Title") {
                    item.text = curPage.Title;
                }
                if(item.name == "Description") {
                    item.text = "操作步骤";
                }
                if(item.name == "DoneCount") {
                    item.text =  "0/" + steps.Count;
                }
            }
            UpBtn.SetActive(true);
            DownBtn.SetActive(true);
            
        }
    }
}