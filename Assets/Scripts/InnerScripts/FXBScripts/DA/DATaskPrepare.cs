using Framework;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using Doozy.Engine;

namespace Fxb.CMSVR
{
    /// <summary>
    /// 处理任务开始前的一些准备流程 目前只是播放一系列动画
    /// </summary>
    public class DATaskPrepare : MonoBehaviour
    {
        public string carName;

        [Tooltip("前舱盖")]
        public string carHoodName;

        [Tooltip("升降机")]
        public Transform lift;

        /// <summary>
        /// 升降机支架A
        /// </summary>
        Transform liftHolderA;

        Transform liftHolderB;

        Transform liftHolderC;

        Transform liftHolderD;

        List<string> animateTargetNames;

        Transform car;

        Transform carHood;

        /// <summary>
        /// 前舱盖支撑杆
        /// </summary>
        Transform carHoodHolder;

        private void Awake()
        {
            Message.AddListener<PrepareTaskMessage>(StartPrepare);
        }

        private void OnDestroy()
        {
            Message.RemoveListener<PrepareTaskMessage>(StartPrepare);
        }

        // Start is called before the first frame update
        void Start()
        {
            GatherCarTrans();

            GatherLiftTrans();
        }

        void GatherCarTrans()
        {
            car = GameObject.Find(carName).transform;

            animateTargetNames = new List<string>() { carHoodName };

            Dictionary<int, Transform> targets = new Dictionary<int, Transform>();

            FindChild(car, animateTargetNames, ref targets);

            carHood = targets[0];

            carHoodHolder = carHood.GetChild(0).GetChild(0);
        }

        void GatherLiftTrans()
        {
            liftHolderA = lift.GetChild(1);

            liftHolderB = lift.GetChild(2);

            liftHolderC = lift.GetChild(3);

            liftHolderD = lift.GetChild(4);
        }

        public void StartPrepare(PrepareTaskMessage msg)
        {
            Sequence sequence = DOTween.Sequence();

            //顺序 举升机支架 - 车舱盖 - 支撑杆 -  任务开始
            sequence.Append(
                liftHolderA.DOLocalRotate(new Vector3(0, 50, 0), 1f, RotateMode.LocalAxisAdd)).Join(
                liftHolderB.DOLocalRotate(new Vector3(0, -50, 0), 1f, RotateMode.LocalAxisAdd)).Join(
                liftHolderC.DOLocalRotate(new Vector3(0, -50, 0), 1f, RotateMode.LocalAxisAdd)).Join(
                liftHolderD.DOLocalRotate(new Vector3(0, 50, 0), 1f, RotateMode.LocalAxisAdd))
                //.AppendInterval(0.5f).Append(
                //lift.DOLocalMoveY(0.13f, 2f)).Insert(1.8f, car.DOLocalMoveY(0.11f, 1.7f)).AppendInterval(0.5f)
                .Append(
                carHood.DOLocalRotate(new Vector3(55, 0, 0), 1f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBounce)).
                Append(carHoodHolder.DOLocalRotate(new Vector3(-1.5f, 9.5f, -87.5f), 0.5f, RotateMode.LocalAxisAdd).
                SetEase(Ease.OutQuart)).AppendCallback(() => World.Get<DASceneState>().isTaskPreparing = false);
        }

        #region Helper

        void FindChild(Transform parent, List<string> names, ref Dictionary<int, Transform> prepareTargets)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (prepareTargets.ContainsKey(i))
                    continue;

                var child = parent.Find(names[i]);

                if (child)
                    prepareTargets[i] = child;
            }

            if (prepareTargets.Count == names.Count)
                return;

            for (int i = 0; i < parent.childCount; i++)
            {
                FindChild(parent.GetChild(i), names, ref prepareTargets);

                if (prepareTargets.Count == names.Count)
                    return;
            }
        }

        Transform FindChild(Transform parent, string name)
        {
            var child = parent.Find(name);

            if (child)
                return child;

            for (int i = 0; i < parent.childCount; i++)
            {
                child = FindChild(parent.GetChild(i), name);

                if (child)
                    return child;
            }

            return null;
        }

        #endregion
    }


    public class PrepareTaskMessage : Message { }
}