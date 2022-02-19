using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSRotatorVerticalPart : MonoBehaviour
{
    public Transform QuZhouRef;
    public float MinHeight;
    public float MaxHeight;

    private float tempRotationX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(QuZhouRef == null) return;
        tempRotationX = QuZhouRef.localRotation.x;
        transform.localPosition = new Vector3(0, MinHeight + (MaxHeight - MinHeight) * Mathf.Abs(tempRotationX), 0);
    }
}