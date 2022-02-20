using Framework;
using Fxb.DA;
using UnityEngine;

namespace Fxb.CMSVR
{
    public class QuzhouRotateCondition : StateMatchProcessCondition
    {
        public Transform QuzhouRotate;
        public DAProcessTarget processTarget;
        public override float NumVal => QuzhouRotate.localRotation.x;

        public override DAProcessTarget ProcessTarget => processTarget;

        public override string GetCompareFaildMsg(CompareType faildCompareType)
        {
            return $"曲轴未被旋转到对应角度 quzhou:{NumVal} diff:{Mathf.Abs(NumVal - numParam)}";
        }
    }
}