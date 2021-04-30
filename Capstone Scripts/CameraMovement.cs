using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;



public class CameraMovement : MonoBehaviour
{
    private bool updateFlag;

    public float unitsPerSecond;

    private float rotation;

    private float framelength = 1000 / 30;

    void displaceCamera(float x, float z)
    {
        gameObject.transform.position += new Vector3(x, 0, z);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (unitsPerSecond == 0)
        {
            unitsPerSecond = 1;
        }
        updateFlag = false;
        rotation = 2F * 3.1415F / 4F;
    }

    // Update is called once per frame
    void Update()
    {
      
        float ms = Time.time * 1000.0F;
        ms %= framelength;
        if (ms < 5 && !updateFlag)
        {
            float dx = 0;
            float dy = 0;
            if (Input.GetKey("a"))
            {
                dx += -1 * (float)Math.Sin(rotation) * unitsPerSecond / 30F;
                dy += (float)Math.Cos(rotation) * unitsPerSecond / 30F;
            }
            if (Input.GetKey("d"))
            {
                dx += (float)Math.Sin(rotation) * unitsPerSecond / 30F;
                dy += -1 * (float)Math.Cos(rotation) * unitsPerSecond / 30F;
        }
            if (Input.GetKey("w"))
            {
                dx += (float)Math.Cos(rotation) * unitsPerSecond / 30F;
                dy += (float)Math.Sin(rotation) * unitsPerSecond / 30F;
            }
            if (Input.GetKey("s"))
            {
                dx += -1 * (float)Math.Cos(rotation) * unitsPerSecond / 30F;
                dy += -1 * (float)Math.Sin(rotation) * unitsPerSecond / 30F;
            }
            if (dx != 0 || dy != 0)
            {
            displaceCamera(dx, dy);
            }
            updateFlag = true;
        }
        if (ms > 25)
        {
            updateFlag = false;
        }
    }
}
