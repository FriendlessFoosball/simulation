using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleInitializer : MonoBehaviour
{
    Rigidbody rb;
    ConfigurableJoint cj;

    const float maxAngularVelocity = 30f;

    Vector3 initPos;
    Quaternion initRot;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.localPosition;
        initRot = transform.localRotation;

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.maxAngularVelocity = maxAngularVelocity;

        cj = GetComponent<ConfigurableJoint>();
    }

    public void Reset() {
        transform.localPosition = initPos;
        transform.localRotation = initRot;
        cj.targetPosition = Vector3.zero;
        cj.targetRotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
