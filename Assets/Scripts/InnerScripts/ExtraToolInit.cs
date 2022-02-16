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
            
            yield return new WaitForEndOfFrame();
            
            yield return new WaitForEndOfFrame();

            if(TryGetComponent<DAObjCtr>(out var objCtr))
            {
                objCtr.ForceSwitchState(DA.CmsObjState.Dismantled);
                objCtr.SetActived(true);
                objCtr.SetDisplayMode(DA.CmsDisplayMode.Hide);
                Debug.Log("ExtraToolInit start");
            }
        }
    }
}