using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSRotatorRotatePart : MonoBehaviour
{
    public Transform VerticalRef;
    public Transform thisRotateRef;
    public Transform quzhouRotateRef;

    // Update is called once per frame
    void Update()
    {

        Vector3 verticalRefPos = VerticalRef.position;

        Vector3 thisRotateRefPos = thisRotateRef.position;

        Vector3 quzhouRotateRefPos = quzhouRotateRef.position;

        float verticalDiffAbs = Mathf.Abs(verticalRefPos.y - quzhouRotateRefPos.y);

        float horizontalDiff = verticalRefPos.z - quzhouRotateRefPos.z;

        float angle = Mathf.Atan(horizontalDiff/verticalDiffAbs)*180/Mathf.PI;

        transform.localEulerAngles = new Vector3(angle, 0, 0);

        Vector3 disDiff = quzhouRotateRefPos - thisRotateRefPos;
        transform.position += disDiff;
    }
}
