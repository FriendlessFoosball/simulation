using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle1Controller : MonoBehaviour
{
    private float rotSpeed = 360f;
    private float linSpeed = 8f;

    private float maxDisplacement = 2f;

    private float currentDisplacement = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && currentDisplacement < maxDisplacement)
        {
            Vector3 translation = Vector3.up * linSpeed * Time.deltaTime;
            transform.Translate(translation);

            currentDisplacement += translation.magnitude;
        }

        if (Input.GetKey(KeyCode.S) && currentDisplacement > -maxDisplacement)
        {
            Vector3 translation = Vector3.down * linSpeed * Time.deltaTime;
            transform.Translate(translation);

            currentDisplacement -= translation.magnitude;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * rotSpeed * Time.deltaTime);
        }
    }
}
