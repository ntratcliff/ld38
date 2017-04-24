using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollZoom : MonoBehaviour
{
    public float MinDist = -1f;
    public float MaxDist = 3f;
    public float Speed = 0.1f;
    public float LerpSpeed = 2f;
    private float targetDist;
    // Use this for initialization
    void Start()
    {
        targetDist = -Camera.main.transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate target distance
        targetDist = Mathf.Clamp(targetDist + -Input.GetAxis("Zoom") * Speed, MinDist, MaxDist);

        // set camera distance
        Vector3 camPos = Camera.main.transform.localPosition;
        camPos.z = Mathf.Lerp(camPos.z, -targetDist, Time.deltaTime * LerpSpeed);
        Camera.main.transform.localPosition = camPos;
    }
}
