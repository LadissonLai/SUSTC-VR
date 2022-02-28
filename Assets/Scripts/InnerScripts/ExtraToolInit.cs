using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;

namespace Fxb.CMSVR
{
    public class ExtraToolInit : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake() {
            Message.AddListener<ExtraToolPrepareMessage>(OnExtraToolPrepareMessage);
            gameObject.SetActive(false);
        }

        private void OnDestroy() {
            Message.RemoveListener<ExtraToolPrepareMessage>(OnExtraToolPrepareMessage);
        }

        private void OnExtraToolPrepareMessage(ExtraToolPrepareMessage msg)
        {
            if(TryGetComponent<DAObjCtr>(out var objCtr))
            {
                objCtr.SetDisplayMode(DA.CmsDisplayMode.Hide);
                objCtr.ForceSwitchState(DA.CmsObjState.Dismantled);
                // objCtr.SetActived(true);
                Debug.Log("ExtraToolInit start");
            }
        }
    }
}