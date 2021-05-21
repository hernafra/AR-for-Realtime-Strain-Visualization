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

    // Debugging value, used to see the last value passed in
    public float cval;

    // Holds the last value passed in, used to determine how 
    // much to move
    private float oVal;

    private Gradient g;

    // Variable used in debugging
    // Uncomment if using Update()
    //private float cat = 0;

    public void delete()
    {
        Destroy(gameObject);
    }

    // Used by csvreader.cs to move/recolor a gauge
    // Expects values 0-0.5
    // Displaces gauges downwards relative to nVal
    public void modify(float nVal)
    {
        cval = nVal;
        displace(-nVal);
        color(nVal * 2);
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

    // --IMPORTANT--
    // Comment out when using a .csv reader or live reader
    // Gauges should do NOTHING on their own and just wait for another object to call them
    // For practical use, call displace() and color() or just modify() externally
    /*
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
    }*/

}
