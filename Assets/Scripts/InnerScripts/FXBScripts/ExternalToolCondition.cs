using Framework;
using Fxb.DA;

namespace Fxb.CMSVR
{
    public class ExternalToolCondition : StateMatchProcessCondition
    {
        public string needToolId;
        public DAProcessTarget processTarget;
        public override float NumVal => (int)World.Get<DAObjCtr>(needToolId).State;

        public override DAProcessTarget ProcessTarget => processTarget;

        public override string GetCompareFaildMsg(CompareType faildCompareType)
        {
            return $"工具 {needToolId} 未安装 {World.Get<DAObjCtr>(needToolId).State}";
        }
    }
}