using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

/* =================================
 * Provides two functions that will read from a 
 * config file and populate the environment with
 * guage elements based on input values.
 * One function takes a string as a parameter while
 * the other uses the file_path member
 * =================================

    Public Functions:
    readConfig()
    readConfig(string)
    clearGauges()

    =================================
*/

public class configReader : MonoBehaviour
{
    // Public variable can be edited in Unity window
    // Used by no input readConfig to access a file
    public string file_path = "./Assets/config.ini";




    // Gets 3 floats from the reader and puts them into xyz values of "v"
    // if it reads invalid values or eof, will return false
    // v value should be discarded at that point as any subsequent fields
    // are invalid
    private bool getReaderCoordinates(StreamReader sr, ref Vector3 v)
    {
        string line;
        if ((line = sr.ReadLine()) != null)
        {
            // Splits "line" into ideally a 3 part string array
            var values = line.Split(',');
            // Ensures there are 3 values
            if (values.Length == 3)
            {
                v.x = float.Parse(values[0]); // Converts strings into floats
                v.y = float.Parse(values[1]);
                v.z = float.Parse(values[2]);
                return true;
            }
        }
        return false;
    }

    // Creates a new strain gauge object at the coordinates given by 'v'
    // Strain gauge script content is arbitrary. configReader doesn't care what it
    // appends to the object
    private void createGauge(Vector3 v)
    {
        // Create object
        GameObject newobj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // IMPORTANT: Object moved first
        newobj.transform.Translate(v);

        // IMPORTANT: Object assigned gauge script second
        newobj.AddComponent<gaugeData>();
    }

    // Clears all current strain gauges from digital env and resets static variable
    // gaugecount
    // This funtion requires script gaugeData to have the function resetCount(). May change
    // later to make more independent
    public void clearGauges()
    {
        gaugeData[] gauges = GameObject.FindObjectsOfType<gaugeData>();

        if (gauges.Length != 0) {
            gauges[0].resetCount();
        }
        foreach (gaugeData elem in gauges)
        {
            //elem.viewInfo();
            elem.delete();
            Destroy(elem);
        }
    }

    // Gets a string and opens the file located at ./Assets/______
    // Reads through the file, taking every 3 floats and converting them
    // into a strain gauge location
    public void readConfig(string s)
    {
        
        if (!File.Exists(s))
        {
            Debug.Log("File does not exist");
            return;
        }

        clearGauges();

        StreamReader reader;
        reader = new StreamReader(s);

        Vector3 v;
        v.x = 0; v.y = 0; v.z = 0;

        // If true, then reader not eof and v is populated with new floats
        while (getReaderCoordinates(reader, ref v))
        {
            createGauge(v);
        }
        reader.Close();
    }


    // Overloaded function. Uses object's filename member (./Assets/config.txt by default)
    // instead of passed in string
    public void readConfig()
    {
        
        if (!File.Exists(file_path))
        {
            Debug.Log("File does not exist");
            return;
        }

        clearGauges();

        StreamReader reader;
        reader = new StreamReader(file_path);

        Vector3 v;
        v.x = 0; v.y = 0; v.z = 0;

        while (getReaderCoordinates(reader, ref v))
        {
            createGauge(v);
        }
        reader.Close();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }
}
