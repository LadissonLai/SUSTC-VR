using Doozy.Engine;
using Doozy.Engine.UI;
using Framework;
using Fxb.DA;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;
using VRTK.GrabAttachMechanics;
using VRTKExtensions;

namespace Fxb.CMSVR
{
    /// <summary>
    /// 塞入了大部分逻辑， 待拆
    /// </summary>
    public class DASceneCtr : SceneScript
    {
        public Transform padPos;

        public Transform carPos;

        private DASceneState SceneState => World.Get<DASceneState>();

        private IRecordModel RecordModel => World.Get<IRecordModel>();

        [Header("Debug")]

        public bool ignoreWrenchConditionCheck;

        public bool skipDAToolAnimation;

        public bool skipSafetyEquip;

        public string taskID;

        public DaTaskMode taskState;

        ITaskModel TaskModel => World.Get<ITaskModel>();

        private DAState DaState => World.Get<DAState>();

        IEnumerator Start()
        {

#if UNITY_EDITOR
            Message.AddListener<GuideTipMessage>(OnGuideTipMessage);
#endif

            Message.AddListener<PartsTableDropObjChangeMessage>(OnPartsTableDropObjChangeMessage);

            Message.AddListener<DAObjStateChangeMessage>(OnObjStateChanged);

            Message.AddListener<CarLiftLocationChangedMessages>(OnLiftLocationChanged);

            Message.AddListener<DAToolErrorMessage>(OnDAToolError);
            Message.AddListener<WearEquipmentMessage>(OnWearEquipment);
            Message.AddListener<ReloadDaSceneMessage>(OnReloadDaScene);

            Message.AddListener<BatteryLiftDeviceStateChangeMessage>(OnBatteryLiftStateChanged);

            yield return new WaitForSeconds(1);

            gameObject.AddComponent<DASystem>();

            gameObject.AddComponent<DATaskGuide>();

            yield return null;

            Message.Send(new StartDAModeMessage()
            {
                mode = DAMode.DisassemblyAssembly,

                rootCtrs = new List<AbstractDAObjCtr>()
                {
                    World.Get<DAObjCtr>("14"),
                    World.Get<DAObjCtr>("13"),
                    World.Get<DAObjCtr>("12"),
                    World.Get<DAObjCtr>("8"),
                    World.Get<DAObjCtr>("4"),
                    World.Get<DAObjCtr>("2"),
                    World.Get<DAObjCtr>("1")
                }
            });

            StartCoroutine(CheckTaskCompleted());

            TryInitWithTask();
        }

        private void OnBatteryLiftStateChanged(BatteryLiftDeviceStateChangeMessage msg)
        {
            SceneState.batteryLiftDeviceState = msg.newState;

            RecordModel.Record(RecordStepType.BatteryLift, ((int)msg.byAction).ToString());
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            Message.RemoveListener<GuideTipMessage>(OnGuideTipMessage);
#endif

            Message.RemoveListener<DAObjStateChangeMessage>(OnObjStateChanged);

            Message.RemoveListener<DAToolErrorMessage>(OnDAToolError);

            Message.RemoveListener<PartsTableDropObjChangeMessage>(OnPartsTableDropObjChangeMessage);

            Message.RemoveListener<CarLiftLocationChangedMessages>(OnLiftLocationChanged);

            Message.RemoveListener<WearEquipmentMessage>(OnWearEquipment);

            Message.RemoveListener<ReloadDaSceneMessage>(OnReloadDaScene);

            Message.RemoveListener<BatteryLiftDeviceStateChangeMessage>(OnBatteryLiftStateChanged);

            World.current.Injecter.UnRegist<ITaskModel>();

            var sceneState = World.Get<DASceneState>();

            if (sceneState != null)
                sceneState.isTaskPreparing = true;
        }

        protected override void Awake()
        {
            base.Awake();

            AnimWrenchCtr.partsPreviewPrefab = Resources.Load<GameObject>(PathConfig.PREFAB_PATH_COMBINE_WRENCH_PREVIEW).transform;

            Instantiate(Resources.Load<GameObject>(PathConfig.PREFAB_PATH_PAD), padPos).transform.ResetLocalMatrix();

            Instantiate(Resources.Load<GameObject>(PathConfig.PREFAB_PATH_CAR), carPos).transform.ResetLocalMatrix();

            if (SceneState == null)
                World.current.Injecter.Regist<DASceneState>();
        }

#if UNITY_EDITOR
        private void Update()
        {
            DAConfig.ignoreWrenchConditionCheck = ignoreWrenchConditionCheck;

            DAConfig.skipToolAnimation = skipDAToolAnimation;

            //测试 直接尝试用右手去拆装提示物体
            if (Input.GetKeyDown(KeyCode.P))
            {
                //抓取装备后自动穿上
                var equipmentObj = VRTKHelper.FindGrabedObjCom<EquipmentObjCtr>();

                if (equipmentObj != null)
                {
                    equipmentObj.Wear(true);

                    return;
                }
                 
                if (DaState.tipsInGuiding != null)
                {
                    foreach (var obj in DaState.tipsInGuiding)
                    {
                        if (obj.interactObj == null)
                            continue;

                        if (obj.interactObj.gameObject.activeInHierarchy && obj.interactObj.isTiped)
                        {
                            if (obj.DisplayMode == CmsDisplayMode.PlaceHolder)
                                obj.interactObj.StopUsing(VRTKHelper.LeftHand.GetComponent<VRTK_InteractUse>());
                            else
                                obj.interactObj.StopUsing(VRTKHelper.RightHand.GetComponent<VRTK_InteractUse>());

                            break;
                        }

                        var daObj = obj as DAObjCtr;

                        if (daObj.CloneObjToPickup != null && daObj.CloneObjToPickup.interactObj.isTiped)
                        {
                            if (VRTKHelper.LeftGrab.GetGrabbedObject() != null)
                                break;

                            if (daObj.CloneObjToPickup.interactObj.TryGetComponent<VRTK_BaseGrabAttach>(out var grabAttach))
                            {
                                //抓取点随意的话会导致抓取物体离手掌距离过远
                                grabAttach.precisionGrab = false;
                            }

                            VRTKHelper.ForceGrab(SDK_BaseController.ControllerHand.Left, daObj.CloneObjToPickup.gameObject);

                            break;
                        }
                    }
                }
            }
        }
#endif

        private void OnPartsTableDropObjChangeMessage(PartsTableDropObjChangeMessage message)
        {
            if (message.objOnTable == null)
            {
                //被取走
                SceneState.cloneObjsInTable.Remove(message.propId);
            }
            else
            {
                if (SceneState.cloneObjsInTable.ContainsKey(message.propId))
                {
                    SceneState.cloneObjsInTable[message.propId] = message.objOnTable.ID;
                }
                else
                {
                    SceneState.cloneObjsInTable.Add(message.propId, message.objOnTable.ID);
                }
            }
        }

        private void OnWearEquipment(WearEquipmentMessage msg)
        {
            RecordModel.Record(RecordStepType.Equip, ((int)msg.equipName).ToString());
        }

        private void OnObjStateChanged(DAObjStateChangeMessage msg)
        {
            if (TaskModel != null && TaskModel.IsSubmitAllTask)
                return;

            var cutState = msg.objCtr.State;

            var preState = msg.preState;

            var recordType = RecordStepType.None;

            switch (cutState)
            {
                case CmsObjState.Dismantled:
                    recordType = RecordStepType.Dismantle;
                    break;
                case CmsObjState.Assembled:
                    recordType = RecordStepType.Assemble;
                    break;
                case CmsObjState.Fixed:
                    recordType = RecordStepType.Fix;
                    break;
            }

            RecordModel.Record(recordType, msg.objCtr.ID);
        }

        private void OnDAToolError(DAToolErrorMessage message)
        {
            //Debug.LogWarning("Wrench use error tip:" + message.tipInfo);

            switch (message.daAnimType)
            {
                case AbstractDAScript.DAAnimType.Disassemble:
                    RecordModel.RecordError(RecordStepType.Dismantle, message.daObjID, ErrorRecordType.InvalidTools);
                    break;
                case AbstractDAScript.DAAnimType.Assemble:
                    RecordModel.RecordError(RecordStepType.Assemble, message.daObjID, ErrorRecordType.InvalidTools);
                    break;
                case AbstractDAScript.DAAnimType.Fix:
                    RecordModel.RecordError(RecordStepType.Fix, message.daObjID, ErrorRecordType.InvalidTools);
                    break;
            }
        }

        private void OnGuideTipMessage(GuideTipMessage msg)
        {
            if(!string.IsNullOrEmpty( msg.tip))
                Debug.Log(msg.tip);
        }

        private IEnumerator CheckTaskCompleted()
        {
            var completedDelay = new WaitForSeconds(1.0f);

            while (TaskModel == null)
                yield return completedDelay;

            while (!TaskModel.CheckAllTaskCompleted())
            {
                yield return completedDelay;
            }

            TaskModel.SubmitTask();

            UIView.ShowView(DoozyNamesDB.VIEW_CATEGORY_PAD, DoozyNamesDB.VIEW_PAD_RECORD);

            Popup_Tips.Show("所有任务完成！任务已提交！");
        }

        void TryInitWithTask()
        {
            string newID;

            var daState = World.Get<DASceneState>();

            newID = daState.taskID2Init;

#if UNITY_EDITOR
            newID = string.IsNullOrWhiteSpace(newID) ? taskID : newID;

#endif

            if (string.IsNullOrWhiteSpace(newID))
                return;

            if (daState.taskMode == DaTaskMode.None)
                daState.taskMode = taskState;

            daState.taskID2Init = null;

            bool safetyEquip = true;

#if UNITY_EDITOR

            safetyEquip = !skipSafetyEquip;

#endif
            World.current.Injecter.Regist<ITaskModel>(new TaskModel(new string[] { newID })).Init(safetyEquip);

            UIView.ShowView(DoozyNamesDB.VIEW_CATEGORY_PAD, DoozyNamesDB.VIEW_PAD_TASKDETAIL);

            Message.Send(new PrepareTaskMessage());
        }

        void OnReloadDaScene(ReloadDaSceneMessage msg)
        {
            SceneManager.LoadScene(0);
        }

        private void OnLiftLocationChanged(CarLiftLocationChangedMessages msg)
        {
            if (TaskModel != null && TaskModel.IsSubmitAllTask)
                return;

            var liftLocation = World.Get<CmsCarState>().liftLocation;

            //print(liftLocation);

            //小于3的整数
            if (int.TryParse(liftLocation.ToString(), out int result) && liftLocation < 3)
            {
                if (!World.Get<CmsCarState>().liftUp && liftLocation != 0)
                    liftLocation += 0.5f;

                RecordModel.Record(RecordStepType.LiftCar, liftLocation.ToString());
            }
        }
    }


    public class ReloadDaSceneMessage : Message { }
}

