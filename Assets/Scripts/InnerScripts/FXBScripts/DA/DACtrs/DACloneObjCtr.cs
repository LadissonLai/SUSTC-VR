using DG.Tweening;
using Doozy.Engine;
using Framework;
using System;
using System.Collections;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;
using VRTKExtensions;

namespace Fxb.CMSVR
{
    /// <summary>
    /// 拆裝物体被拆下后生成。
    /// </summary>
    public class DACloneObjCtr : MonoBehaviour, IDAUsingTool
    {
        private Rigidbody rigidBody;

        public bool WaitForFistPick { get; private set; } = true;

        /// <summary>
        /// 是否等待被放置。（没有放置过）
        /// </summary>
        public bool WaitForFistDrop { get; private set; } = true;
         
        public AdvancedInteractableObj interactObj;

        public HandToolCollisionTracker collisionTracker;

        public DAGridPlane dropGridPlane;

        [NonSerialized]
        public bool isGenerateByModelCopy;

        [Tooltip("可以留空，拆下物体后会自动赋值")]
        public string PropID;

        /// <summary>
        /// 自身id 方便统一管理
        /// </summary>
        public string ID { get; protected set; }

        public bool FixUsingAble => false;

        public bool IsGrabed => interactObj.IsGrabbed();

        public bool IsAnimPlaying { get; protected set; }

        public bool IsUsing => false;

        /// <summary>
        /// 可以放置到的目标  零件桌暂时没用此逻辑
        /// </summary>
        public ICloneObjDropAble CurrentValidDropAble { get; protected set; }

        protected DACloneObjCtr nextNodeObj;

        /// <summary>
        /// 安装的时候有可能会批量直接销毁，外部需要能收到对应消息
        /// </summary>
        public event Action<DACloneObjCtr> OnPlaced;

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(ID))
                World.current.Injecter.Regist(this, ID);
        }

        private void OnDisable()
        {
            if (!string.IsNullOrEmpty(ID))
                World.current.Injecter.UnRegist<DACloneObjCtr>(ID);
        }

        private void Awake()
        {
            SetValidDrop(false);

            interactObj.InteractableObjectUngrabbed += InteractObj_InteractableObjectUngrabbed;

            interactObj.InteractableObjectGrabbed += InteractObj_InteractableObjectGrabbed;

            interactObj.InteractableObjectTryDrop += InteractObj_InteractableObjectTryDrop;

            interactObj.isUsable = true;

            interactObj.useOnlyIfGrabbed = false;

            interactObj.holdButtonToUse = true;
        }

        private void InteractObj_InteractableObjectTryDrop(AdvancedInteractableObj arg1, bool isDropable)
        {
            if(!isDropable)
            {
                var errorMsg = "请将拆下的物体放到零件桌的合适位置";

                if (dropGridPlane == null)
                {
                    //需要放到桌上
                    errorMsg = "请将拆下的物体放到指定位置";
                }

                Popup_Tips.Show(errorMsg);
            }
        }

        private void Start()
        {
            if (interactObj.grabAttachMechanicScript == null)
            {
                if(!interactObj.TryGetComponent<VRTK_BaseGrabAttach>(out var grabAttach))
                {
                    grabAttach = interactObj.gameObject.AddComponent<VRTK_ChildOfControllerGrabAttach>();

                    grabAttach.precisionGrab = true;
                }

                interactObj.grabAttachMechanicScript = grabAttach;
            }

            ID = $"{PropID}_{DateTime.Now.Ticks}_{this.GetHashCode()}";

            name = ID;

            World.current.Injecter.Regist(this, ID);

            collisionTracker.CustomPredicate = CollisionPredicate;

            if (dropGridPlane == null)
                dropGridPlane = GetComponentInChildren<DAGridPlane>();
                // dropGridPlane = GameObject.Find("DropAblePlane").GetComponent<DAGridPlane>();

            rigidBody = GetComponent<Rigidbody>();

            if (rigidBody == null)
            {
                rigidBody = gameObject.AddComponent<Rigidbody>();

                rigidBody.isKinematic = true;

                rigidBody.useGravity = false;
            }

            if (!isGenerateByModelCopy)
                return;

            var colliders = GetComponentsInChildren<Collider>();

            for (int i = colliders.Length - 1; i >= 0; i--)
            {
                var c = colliders[i];

                c.enabled = true;

                //拷贝出来的模型，有可能会带上拆装物体单独创建的trigger碰撞体。
                if (c.isTrigger)
                    Destroy(c);
            }
        }
 
        private void InteractObj_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
        {
            Drop();
        }

        private void InteractObj_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
        {
            Pickup();
        }

        /// <summary>
        /// 不通过此条件不会触发目标物体高亮，也无法使用
        /// TODO 零件桌的放置逻辑没有走此机制，待修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CollisionPredicate(AdvancedInteractableObj obj)
        {
            if(obj.TryGetComponent<ICloneObjDropAble>(out var dropAbleTarget))
            {
                if (dropAbleTarget.CheckCloneObjDropAble(this))
                    return true;
            }
             
            return false;
        }

        private IEnumerator PlayFlyAnim(DACloneObjCtr other)
        {
            IsAnimPlaying = other.IsAnimPlaying = true;

            while (true)
            {
                //中途被抓取则取消动画
                if (other.IsGrabed)
                    yield break;

                var fromPos = other.transform.position;

                var fromRot = other.transform.rotation;

                var toPos = transform.position;

                var toRot = transform.rotation;

                var distance = Vector3.Distance(fromPos, toPos);

                var angle = Quaternion.Angle(fromRot, toRot);

                if (distance < 0.02f && angle < 2f)
                    break;

                //Debug.Log($"dis:{distance}  angle:{angle}");

                other.transform.position = Vector3.Lerp(fromPos, toPos, 0.2f);
                other.transform.rotation = Quaternion.Lerp(fromRot, toRot, 0.1f);

                yield return null;
            }

            IsAnimPlaying = other.IsAnimPlaying = false;

            AddCloneObj(other, false);
        }

        protected DACloneObjCtr GetLastNodeObj(int loopCount = 0)
        {
            var tmpNextObj = nextNodeObj;

            if (++loopCount > 20)
            {
                Debug.LogError($"LastNodeObj Get error:{name}");

                return this;
            }

            if (nextNodeObj == null)
                return this;

            return nextNodeObj.GetLastNodeObj(loopCount);
        }

        /// <summary>
        /// 由外部调用  放置到拆装物体身上
        /// 期望在销毁前能够通知外部做清理
        /// </summary>
        public void Place()
        {
            Debug.Assert(GetAmount() == 1);

            OnPlaced?.Invoke(this);

            gameObject.SetActive(false);

            //先关闭再调用ForceStopInteracting，否则会延迟一帧处理stop interacting相关逻辑
            interactObj.ForceStopInteracting();

            Destroy(gameObject);
        }

        protected void Drop()
        {
            WaitForFistDrop = false;

            if (nextNodeObj != null)
                nextNodeObj.Drop();
        }

        protected void Pickup()
        {
            Message.Send(new CloneObjPickupMessage() { target = this });

            WaitForFistPick = false;
        }

        public int GetAmount()
        {
            if (nextNodeObj == null)
                return 1;

            //检测链表内循环的问题
            var slow = this;
            var fast = this;

            var amount = 0;

            while (slow.nextNodeObj != null)
            {
                slow = slow.nextNodeObj;

                amount++;

                if (fast.nextNodeObj != null && fast.nextNodeObj.nextNodeObj != null)
                {
                    fast = fast.nextNodeObj.nextNodeObj;

                    if (fast == slow)
                    {
                        //内循环
                        Debug.LogError("内循环");
                        return 1;
                    }
                }
            }

            return amount + 1;
        }

        public void SetValidDrop(bool state, ICloneObjDropAble targetDropAble = null)
        {
            if (this.CurrentValidDropAble != null && this.CurrentValidDropAble != targetDropAble && !state )
                return;

            if (state)
                this.CurrentValidDropAble = targetDropAble;
            else if (targetDropAble == this.CurrentValidDropAble)
                this.CurrentValidDropAble = null;
            //有可能需要放置到指定位置 TODO
            interactObj.validDrop = state ? VRTK_InteractableObject.ValidDropTypes.DropAnywhere : VRTK_InteractableObject.ValidDropTypes.NoDrop;
        }

        /// <summary>
        /// 待实现 取消螺丝合并逻辑
        /// </summary>
        private void UpdateAmountState()
        {
            var amount = GetAmount();

            Debug.Log("new amount:" + amount);

            //interactObj.readableName = $""
        }

        public void AddCloneObj(DACloneObjCtr other, bool playFlyAnim = false)
        {
            if (other.transform != null)
                other.transform.SetParent(null);

            if (playFlyAnim)
            {
                World.current.StartCoroutine(PlayFlyAnim(other));
            }
            else
            {
                other.gameObject.SetActive(false);

                //新增物体放到最后
                var lastNodeObj = GetLastNodeObj();

                lastNodeObj.nextNodeObj = other;

                if (IsGrabed)
                {
                    //自身被抓取
                    other.Pickup();
                }

                //UpdateAmountState();
            }
        }

        /// <summary>
        /// next保留 自身分离
        /// </summary>
        /// <returns></returns>
        public DACloneObjCtr SeparateCurrent()
        {
            var next = nextNodeObj;

            if (next == null)
                return null;
 
            next.transform.position = transform.position;

            next.transform.rotation = transform.rotation;

            next.gameObject.SetActive(true);

            nextNodeObj = null;

            return next;
        }

        /// <summary>
        /// 分离出nextObj，自身保留
        /// </summary>
        public DACloneObjCtr SeparateNext()
        {
            var next = nextNodeObj;

            if (next == null)
                return null;
             
            nextNodeObj = next.nextNodeObj;

            next.nextNodeObj = null;

            next.transform.position = transform.position;

            next.transform.rotation = transform.rotation;

            next.gameObject.SetActive(true);

            return next;
        }
    }
}



