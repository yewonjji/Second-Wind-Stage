using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMotion : MonoBehaviour
{

    [Range(1f,10f)]
    public float speed = 1.0f;
    public float multiplier = 1.0f;
    public float offset = 0.0f;

    Quaternion startingRotation;
    float waveAngle;
    void Start ()
    {
        //파동 움직임 원점
        startingRotation = transform.rotation;
    }
	void Update ()
    {
        waveAngle = Mathf.Sin(Time.time * speed + offset) * multiplier;
        transform.rotation = startingRotation;
        transform.RotateAround(transform.position, Vector3.forward, waveAngle);
    }
}



