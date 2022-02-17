using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fxb.CMSVR
{
    public class ExtraToolInit : MonoBehaviour
    {
        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return new WaitForSeconds(1.5f);

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