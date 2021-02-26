using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle2Controller : MonoBehaviour
{
    private float rotSpeed = 360f;
    private float linSpeed = 8f;

    private float maxDisplacement = 2f;

    private float currentDisplacement = 0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 translation = Vector3.down * Input.GetAxis("Vertical") * linSpeed * Time.deltaTime;
        transform.Translate(translation);

        Vector3 rotation = Vector3.up * Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.Rotate(rotation);


        /*if (Input.GetKey(KeyCode.UpArrow) && currentDisplacement < maxDisplacement)
        {
            Vector3 translation = Vector3.down * linSpeed * Time.deltaTime;
            transform.Translate(translation);

            currentDisplacement += translation.magnitude;
        }

        if (Input.GetKey(KeyCode.DownArrow) && currentDisplacement > -maxDisplacement)
        {
            Vector3 translation = Vector3.up * linSpeed * Time.deltaTime;
            transform.Translate(translation);

            currentDisplacement -= translation.magnitude;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.up * rotSpeed * Time.deltaTime);
        }
        */
    }
}
