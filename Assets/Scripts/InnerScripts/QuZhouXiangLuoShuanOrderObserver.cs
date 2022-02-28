using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using Fxb.DA;
namespace Fxb.CMSVR
{
    public class QuZhouXiangLuoShuanOrderObserver : MonoBehaviour
    {
        private HashSet<string> disassembledIDs;

        private bool isEnabled = true;

        private 
        // Start is called before the first frame update
        void Start()
        {
            if(EntrySetting.Instance != null && EntrySetting.Instance.behaviour == Enums.Behaviour.Disassembly)
            {
                disassembledIDs = new HashSet<string>();
                Message.AddListener<DAObjStateChangeMessage>(OnDAObjStateChangeMessage);
            }
            else
            {
                Destroy(this);
            }
        }

        // Update is called once per frame
        void OnDestroy()
        {
            Message.RemoveListener<DAObjStateChangeMessage>(OnDAObjStateChangeMessage);
        }

        void OnDAObjStateChangeMessage(DAObjStateChangeMessage msg)
        {
            if(!isEnabled)
            {
                Message.RemoveListener<DAObjStateChangeMessage>(OnDAObjStateChangeMessage);
                return;
            }
            if(msg.objCtr.ID.StartsWith("702_0"))
            {
                if(msg.objCtr.State == CmsObjState.Dismantled)
                {
                    disassembledIDs.Add(msg.objCtr.ID);
                }
                else
                {
                    disassembledIDs.Remove(msg.objCtr.ID);
                }
            }
            else if(msg.objCtr.ID.StartsWith("702_1"))
            {
                if(msg.objCtr.State == CmsObjState.Dismantled)
                {
                    isEnabled = false;
                    if(disassembledIDs.Count < 13)
                    {
                        Message.Send(new DAErrorMessage("两种螺栓混合拆卸", "702_2", AbstractDAScript.DAAnimType.None, "70201"));
                    }
                }
            }
        }
    }
}