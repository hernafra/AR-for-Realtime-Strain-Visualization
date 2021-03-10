using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gaugeData : MonoBehaviour
{
    // Shared variable for the class
    // Used to know how many gauges there are
    static private int objCount = 0;

    // Used to track gauges. Each id wil be paired with
    // which gauge value appears in the .csv file in order
    public int gaugeId;

    public float x0;
    public float y0;
    public float z0;

    public float x1;
    public float y1;
    public float z1;

    private Gradient g;

    public void resetCount()
    {
        objCount = 0;
    }

    public void delete()
    {
        Destroy(gameObject);
    }

    public void displace1(float x, float y, float z)
    {
        x1 += x;
        y1 += y;
        z1 += z;

        gameObject.transform.position += new Vector3(x, y, z);
    }

    public void displace0(float x, float y, float z)
    {
        x1 = x0 + x;
        y1 = y0 + y;
        z1 = z0 + z;

        gameObject.transform.position = new Vector3(x1, y1, z1);
    }

    public void color(float value)
    {
        var Renderer = this.GetComponent<Renderer>();
        Renderer.material.color = g.Evaluate(value);
    }

    public void viewInfo()
    {
        Debug.Log("ID: " + gaugeId);
        Debug.Log("x0: " + x0);
        Debug.Log("x1: " + x1);
        Debug.Log("y0: " + y0);
        Debug.Log("y1: " + y1);
        Debug.Log("z0: " + z0);
        Debug.Log("z1: " + z1);
    }



    // Init function to set individual gauge id
    private void initId()
    {
        gaugeId = objCount++;
    }

    // Init function to set initial positions of gauges
    // note: object attached to gauge script should be
    // moved to initial position before calling initPosition()
    public void initPosition()
    {
        Vector3 v = gameObject.transform.position;
        x0 = v.x;
        y0 = v.y;
        z0 = v.z;
        x1 = x0;
        y1 = y0;
        z1 = z0;
    }

    // Start is called before the first frame update
    void Start()
    {
        initId();
        initPosition();

        //Debug.Log("creating gradient");

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

}
