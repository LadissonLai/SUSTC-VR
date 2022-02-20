using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fxb.DA;
using Framework;
using Fxb.CMSVR;

public class QuBingLianGanClone : MonoBehaviour
{
    public GameObject QuBingLianGan_Origin;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);

        if(World.Get<ITaskModel>() == null)
        {
            gameObject.SetActive(false);
            yield return null;
        }
        int taskId = int.Parse(World.Get<ITaskModel>()?.GetData()[0].taskID);
        if(taskId == 6)
        {
            QuBingLianGan_Origin.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
