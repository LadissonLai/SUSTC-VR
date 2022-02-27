using Doozy.Engine;
using static Fxb.DA.AbstractDAScript;

namespace Fxb.DA
{
    /// <summary>
    /// 拆装工具使用错误消息。 待从Wrench文件夹中移出
    /// </summary>
    public class DAErrorMessage : Message
    {
        public string tipInfo;

        public string daObjID;

        public DAAnimType daAnimType;

        public string errorID;

        public DAErrorMessage(string tipInfo, string daObjID, DAAnimType daAnimType, string errorID)
        {
            this.tipInfo = tipInfo;

            this.daObjID = daObjID;

            this.daAnimType = daAnimType;

            this.errorID = errorID;
        }
    }
}
