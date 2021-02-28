using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleInitializer : MonoBehaviour
{
    Rigidbody rb;

    const float maxAngularVelocity = 30f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        //rb.maxAngularVelocity = maxAngularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
