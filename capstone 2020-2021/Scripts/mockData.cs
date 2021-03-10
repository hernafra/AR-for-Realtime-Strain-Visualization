using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;


// Description:
// Put this script onto any object in the scene and it will create mock data and
// place it it Assets/mockdata.csv
// There are default parameters that can be edited in the Unity window



public class mockData : MonoBehaviour
{
    public int numGauges;
    public int minVal;
    public int maxVal;
    public float valueIncrease;


    public int updateIntMs;

    public string initPattern;
    public string growthPattern;


    private List<float> data;

    // Used to tell if csv file has been updated in current cycle
    // True means it has, false doesn't necessarily mean it hasn't
    private bool updateFlag;

    // Debugging: prints values of the data member
    void printData()
    {
        for (int i = 0; i < data.Count; i++)
        {
            Debug.Log(data[i]);
        }
    }

    void initConstant()
    {
        float avg = (float)((minVal + maxVal) / 2.0);
        for (int i = 0; i < numGauges; i++)
        {
            data.Add(avg);
        }
    }


    void initLinear()
    {
        int diff = maxVal - minVal + 1;
        for (int i = 0; i < numGauges; i++)
        {
            data.Add((float)((10f * i * valueIncrease) % diff + minVal));
        }
    }

    void initSin()
    {
        float avg = (float)((minVal + maxVal) / 2.0);
        float amp = (float)(maxVal - avg);
        for (int i = 0; i < numGauges; i++)
        {
            float interval = (float)i / (float)10.0;
            float s = Mathf.Sin((float)(2.0 * 3.1415 * interval));
            data.Add(s * amp + avg);
        }
    }

    protected virtual bool IsFileLocked(FileInfo file)
    {
        try
        {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }

    void updateLinear()
    {
        var fi = new FileInfo("./Assets/mockdata.csv");
        if (!IsFileLocked(fi))
        {

            StreamWriter sw = new StreamWriter("./Assets/mockdata.csv");

            int diff = maxVal - minVal + 1;
            for (int i = 0; i < data.Count; i++)
            {
                data[i] = data[i] + valueIncrease;
                if (data[i] > maxVal)
                {
                    data[i] -= diff;
                }
                sw.WriteLine(i + "," + data[i]);
            }
            sw.Close();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        data = new List<float>();
        updateFlag = false;


        if (numGauges == 0)
            numGauges = 1;
        if (minVal >= maxVal)
            maxVal++;
        if (updateIntMs < 10 || updateIntMs > 5000)
            updateIntMs = 20;

        if (initPattern == "Linear")
        {
            initLinear();
        }
        else if (initPattern == "Sin")
        {
            initSin();
        }
        else
        {
            initConstant();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (growthPattern == "Linear") { 
            float ms = Time.time * 1000.0F;
            ms %= updateIntMs;
            if (ms < updateIntMs * .5 && !updateFlag)
            {
                updateLinear();
                updateFlag = true;
            }
            if (ms > updateIntMs * .5)
            {
                updateFlag = false;
            }
        }
    }
}
