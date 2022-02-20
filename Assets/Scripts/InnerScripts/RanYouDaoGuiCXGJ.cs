using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fxb.CMSVR;

public class RanYouDaoGuiCXGJ : MonoBehaviour
{
    private bool isAssembled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAssembled && TryGetComponent<DAObjCtr>(out var component) && component.State == Fxb.DA.CmsObjState.Default && TryGetComponent<Collider>(out var collider))
        {
            Destroy(collider);
            isAssembled = true;
            Destroy(this);
        }
    }
}
