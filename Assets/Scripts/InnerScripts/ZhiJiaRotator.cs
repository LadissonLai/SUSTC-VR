using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhiJiaRotator : MonoBehaviour
{
    public Transform rotationRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float refAngle = GetInpectorEulers(rotationRef).z;
        Vector3 origin = transform.localEulerAngles;
        // Vector3 origin = GetInpectorEulers(transform);
        // transform.localRotation = Quaternion.Euler(new Vector3(refAngle, origin.y, origin.z));
        transform.localEulerAngles = new Vector3(refAngle, origin.y, origin.z);
    }

     private Vector3 GetInpectorEulers(Transform mTransform)
    {
        Vector3 angle = mTransform.eulerAngles;
        float x = angle.x;
        float y = angle.y;
        float z = angle.z;
 
        if (Vector3.Dot(mTransform.up, Vector3.up) >= 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = angle.x - 360f;
            }
        }
        if (Vector3.Dot(mTransform.up, Vector3.up) < 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = 180 - angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = 180 - angle.x;
            }
        }
 
        if (angle.y > 180)
        {
            y = angle.y - 360f;
        }
 
        if (angle.z > 180)
        {
            z = angle.z - 360f;
        }
        Vector3 vector3 = new Vector3(Mathf.Round(x), Mathf.Round(y), Mathf.Round(z));
        //Debug.Log(" Inspector Euler:  " + Mathf.Round(x) + " , " + Mathf.Round(y) + " , " + Mathf.Round(z));
        return vector3;

    }
}
