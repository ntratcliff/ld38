using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CapsuleRotationController : MonoBehaviour
{
    public Vector3 RotationMultipliers = new Vector3(1f, 1f, 1f);
    public float GrabMagnitude = 2;

    private Vector3 lastMousePos;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        lastMousePos = Input.mousePosition;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // reset rotational velocity if mouse is first pressed
        if (Input.GetMouseButtonDown(0))
        {
            rb.angularVelocity /= GrabMagnitude;
        }

        // check if mouse is down
        if (Input.GetMouseButton(0))
        {
            // get delta
            Vector3 mouseDelta = Input.mousePosition - lastMousePos;

            // map delta movements to corresponding axis
            // mouse x -> y rot
            // mouse y -> x rot
            Vector3 torque = new Vector3(
                mouseDelta.y * RotationMultipliers.x, 
                mouseDelta.x * RotationMultipliers.y,
                mouseDelta.z * RotationMultipliers.z);

            // spin based on delta
            rb.AddTorque(torque, ForceMode.Acceleration);
        }

        lastMousePos = Input.mousePosition;
    }
}
