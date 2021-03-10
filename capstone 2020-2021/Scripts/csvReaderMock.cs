using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class csvReaderMock : MonoBehaviour
{
    StreamReader reader;

    float timeoffset;

    bool pause;


    public void togglePause()
    {
        pause = !pause;
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

    public void readData()
    {
        var fi = new FileInfo("./Assets/mockdata.csv");
        if (!IsFileLocked(fi))
        {
            float ms = Time.time * 1000.0F;
            reader = new StreamReader("./Assets/mockdata.csv");
            string line;

            gaugeData[] gauges = GameObject.FindObjectsOfType<gaugeData>();

            int gaugeindex = gauges.Length - 1;
            if (gaugeindex >= 0)
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    gaugeData target = gauges[gaugeindex--];

                    target.displace0(0, float.Parse(values[1]), 0);
                    target.color(float.Parse(values[1]));
                }
            }
            reader.Close();
        }
        else
        {
            timeoffset += 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause) { 
            float ms = Time.time * 1000.0F + timeoffset;
            ms %= 100;
            if (ms > 50)
            {
                readData();
            }
        }
    }
}
