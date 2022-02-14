using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRotator : MonoBehaviour
{
    public Transform rotationRef;

    public Transform ZhiJiaPoint;

    public Transform EnginePoint;
    
    private Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.localEulerAngles;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float refAngle = rotationRef.localEulerAngles.z;
        transform.localEulerAngles = new Vector3(-refAngle, origin.y, origin.z);

        Vector3 disDiff = ZhiJiaPoint.position - EnginePoint.position;
        transform.position += disDiff;
    }
}
