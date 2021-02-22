using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle1Controller : MonoBehaviour
{
    [SerializeField]
    private float speed = 360f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.up * speed * Time.deltaTime);
        }
    }
}
