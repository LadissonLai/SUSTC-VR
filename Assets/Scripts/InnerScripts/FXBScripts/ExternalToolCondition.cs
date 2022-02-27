using Framework;
using Fxb.DA;
using Doozy.Engine;

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
            var daObjID = this.GetComponent<AbstractDAScript>().daObjID;
            var daAnimType = AbstractDAScript.DAAnimType.None;
            switch (processTarget)
            {
                case DAProcessTarget.None:
                    daAnimType = AbstractDAScript.DAAnimType.None;
                    break;
                case DAProcessTarget.Assemble:
                    daAnimType = AbstractDAScript.DAAnimType.Assemble;
                    break;
                case DAProcessTarget.Dismantle:
                    daAnimType = AbstractDAScript.DAAnimType.Disassemble;
                    break;
                case DAProcessTarget.Fix:
                    daAnimType = AbstractDAScript.DAAnimType.Fix;
                    break;
            }

            var errorID = "";
            switch (daObjID)
            {
                case "11_3":
                    errorID = "11101";
                    break;
                case "203_0":
                    errorID = "20301";
                    break;
                case "206_3":
                    errorID = "20601";
                    break;
            }
            Message.Send(new DAErrorMessage("请安装正确的工具后进行操作", daObjID, daAnimType, errorID));

            return $"工具 {needToolId} 未安装 {World.Get<DAObjCtr>(needToolId).State}";
        }
    }
}