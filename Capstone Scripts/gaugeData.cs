using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class gaugeData : MonoBehaviour
{
    // Used to track gauges. Each id wil be paired with
    // which gauge value appears in the .csv file in order
    // Set MANUALLY in Unity scene
    public int gaugeId;

    private float oVal;

    private Gradient g;

    // Variable used in debugging, ignore
    private float cat = 0;

    public void delete()
    {
        Destroy(gameObject);
    }

    // Pass values mindisplaecment-maxdisplacement (can be sub-zero)
    // Probably keep |nVal| < ~.5 or else gauge moves REALLY far
    // Up to the .csv reader to call, gauges won't move otherwise
    public void displace(float nVal)
    {
        // Gets the difference between the PREVIOUS and CURRENT positions
        float dy = nVal - oVal;
        
        // Moves the object along the y axis depending on dy
        // Space.Self makes it so object moves up in MODEL coordinates
        // Without, would move up/down regardless of rotation
        gameObject.transform.Translate(0, dy, 0, Space.Self);

        // Saves the current positions
        oVal = nVal;
    }

    // Pass values 0-1
    public void color(float value)
    {
        var Renderer = this.GetComponent<Renderer>();
        Renderer.material.color = g.Evaluate(value);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Assumes that object is at zeroe'd position
        oVal = 0;

        // Set up gradient object
        g = new Gradient();
        GradientColorKey[] gck = new GradientColorKey[2];
        GradientAlphaKey[] gak = new GradientAlphaKey[2];
        gck[0].color = Color.red;
        gck[0].time = 1.0F;
        gck[1].color = Color.blue;
        gck[1].time = 0.0F;
        gak[0].alpha = 0.0F;
        gak[0].time = 1.0F;
        gak[1].alpha = 0.0F;
        gak[1].time = 0.0F;
        g.SetKeys(gck, gak);
    }

    //IMPORTANT
    // Comment out when using a .csv reader
    // For practical use, call displace() and color() externally
    void Update()
    {
        if (oVal < .2)
        {
            color(cat * 5);
            displace(cat);
            cat += 0.0002f;
        }
        else
        {
            color(0);
            displace(0);
            cat = 0;
        }
    }

}
