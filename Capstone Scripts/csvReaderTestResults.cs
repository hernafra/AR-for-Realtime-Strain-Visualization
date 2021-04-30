using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class csvReaderTestResults : MonoBehaviour
{

    public int readerOffset = 7;
    public string filename = "./Assets/ARRSV Example Data.csv";

    public int numValues = 21;


    private StreamReader SR;
    private List<List<float>> data;
    private float timeOffset = 0;

    private int dataIndex = 0;

    float getTime(string s)
    {
        var values = s.Split(':');
        float prevTime = 0;


        // Gets (minutes * 60 + seconds) to get total seconds
        float val = float.Parse(values[0]) * 60 + float.Parse(values[1]);

        // Zero's the current time if at start of list
        if (timeOffset == 0)
        {
            timeOffset = val;
        }

        // Slightly increments this entry if previous one is at the same time
        if (prevTime >= val)
        {
            val = prevTime + 0.1f;
        }

        // Reassigns the prevtime variable
        prevTime = val;

        return val - timeOffset;
    }

    void writeLine(string s)
    {
        var values = s.Split(',');

        List<float> temp = new List<float>();

        temp.Add(getTime(values[0]));

        for (int i = 1; i < numValues; i++)
        {
            temp.Add(float.Parse(values[i]));
        }
        data.Add(temp);
    }

    void printRow(int n)
    {
        string mes = "";
        for (int i = 0; i < numValues; i++)
        {
            mes += data[n][i] + " ";
        }
        Debug.Log(mes);
    }

    void printInfo()
    {
        Debug.Log(data.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        data = new List<List<float>>();
        SR = new StreamReader(filename);

        for (int i = 0; i < readerOffset; i++)
        {
            SR.ReadLine();
        }

        string buffer;
        while ((buffer = SR.ReadLine()) != null)
        {
            writeLine(buffer);
        }
    }
    
    void Update()
    {
        
        float ms = Time.time;

        if (ms >= data[dataIndex][0]) { 


        //8 to 15

            gaugeData[] gauges = GameObject.FindObjectsOfType<gaugeData>();

            if (gauges.Length > 0) {
                for (int i = 15; i >= 8; i--) { 

                    gaugeData target = gauges[i - 8];

                    target.displace0(0, data[dataIndex][i], 0);
                    target.color(data[dataIndex][i]);
                }
            }
            dataIndex++;
        }




    }
    
}
