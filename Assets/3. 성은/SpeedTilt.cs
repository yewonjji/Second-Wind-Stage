using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedTilt : MonoBehaviour
{

    public float multiplier = 1.0f;
    public float tiltDamp = 0.1f;

    Quaternion startingRotation;
    float currentTilt;
    float targetTilt;
    float  velocityTilt;

    float  currentXspeed ;
    float previousXPosition ;

    void Start ()
    {
        startingRotation = GetComponent<Transform>().rotation;
    }

    void Update ()
    {
        currentXspeed = GetComponent<Transform>().position.x - previousXPosition;
        targetTilt = Mathf.Clamp(currentXspeed * multiplier, -90, 90);
        currentTilt = Mathf.SmoothDamp(currentTilt, targetTilt, ref velocityTilt, tiltDamp);
        previousXPosition = GetComponent<Transform>().position.x;
        GetComponent<Transform>().rotation = startingRotation;
        GetComponent<Transform>().RotateAround(GetComponent<Transform>().position, 
                                                                                 Vector3.forward, currentTilt);
    }
}





