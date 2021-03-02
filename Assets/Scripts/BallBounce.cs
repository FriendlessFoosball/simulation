using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnTriggerEnter(Collider collision) {
    //     if (collision.gameObject.CompareTag("Wall")) {
    //         Vector3 p = collision.gameObject.transform.localPosition;
    //         Vector3 normal = Mathf.Abs(p.z) > 0.5f
    //             ? Vector3.back * Mathf.Sign(p.z)
    //             : Vector3.left * Mathf.Sign(p.x);
    //         rb.velocity = Vector3.Reflect(rb.velocity, normal) * 0.9f;
    //     }
    // }
}
