using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSRotatorVerticalPart : MonoBehaviour
{
    public Transform QuZhouRef;
    public bool isLow;

    private float startHeight;
    private float Height = 0.14f;

    private float tempRotationX;

    // Start is called before the first frame update
    void Start()
    {
        startHeight = transform.localPosition.y;
        Height = isLow ? Height : -Height;
    }

    // Update is called once per frame
    void Update()
    {
        if(QuZhouRef == null) return;
        tempRotationX = QuZhouRef.localRotation.x;
        transform.localPosition = new Vector3(0, startHeight + Height * Mathf.Abs(tempRotationX), 0);
    }
}