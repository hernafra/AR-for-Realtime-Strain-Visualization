using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

/* =================================
 * API for creating new strain gauge templates
 * Doesn't have way to get user information to
 * create strain gauge locations
 * 
 * Can be used by user to create new templates
 * as well as let the team create the default
 * template without physically measuring test site
 * =================================

    Public Functions:
    setFileLocation()
    SwapGauges()
    addGauge()
    modGauge()
    clearWriter()
    writeFile()
    
    =================================
*/

public class configWriter : MonoBehaviour
{

    private List<Vector3> coordinates;
    private string filename;

    // Prints out filename to console
    // Filename will not include "./Assets", but all files
    // are automatically placed there
    void viewFilename()
    {
        Debug.Log("Location = ./Assets/" + filename);
    }

    // Views the number of gauges currently queued to be saved
    void viewNumGauges()
    {
        Debug.Log("Queued gauges = " + coordinates.Count);
    }

    // Views the index (id) and location of each gauge in xyz coordinates
    void viewGauges()
    {
        for (int i = 0; i < coordinates.Count; i++)
        {
            Debug.Log("Gauge " + (i + 1) + ": (" + coordinates[i].x + ", " + coordinates[i].y + ", " + coordinates[i].z + ")\n");
        }
    }

    //========================================================================

    // Returns a vec3 with x, y, z values provided
    Vector3 setVec3(float x, float y, float z)
    {
        Vector3 tmp;
        tmp.x = x;
        tmp.y = y;
        tmp.z = z;
        return tmp;
    }

    // Used to set the filename for saving
    // Note: ./Assets will be appended to the start
    public void setFileLocation(string s)
    {
        filename = s;
    }

    // Used to swap index of two gauges
    // (or swap the position of two gauges)
    public void SwapGauges(int i1, int i2)
    {
        Vector3 tmp = coordinates[i1];
        coordinates[i1] = coordinates[i2];
        coordinates[i2] = tmp;
    }

    // Adds a new gauge at a given location
    // Note: may consider breaking into two functions:
    // 1. Spawn a gauge at 0, 0, 0
    // 2. Use mod gauge function to move gauge
    // May be more user friendly/easier to implement interaction
    public void addGauge(float x, float y, float z)
    {
        Vector3 tmp = setVec3(x, y, z);
        coordinates.Add(tmp);
    }

    // Moves a gauge's location
    public void modGauge(float x, float y, float z, int idx)
    {
        Vector3 tmp = setVec3(x, y, z);
        coordinates[idx] = tmp;
    }

    // Clears the gauge queue and filename
    public void ClearWriter()
    {
        coordinates.Clear();
        filename = null;
    }

    // Writes the .ini file with provided settings at given filepath
    // Fails if no gauges included or filepath not specified
    // Note: Will overwrite existing file if sharing names
    public void writeFile()
    {
        if (filename == null || coordinates.Count == 0)
        {
            return;
        }

        StreamWriter sw = new StreamWriter("./Assets/" + filename);
        for(int i = 0; i < coordinates.Count; i++)
        {
            sw.WriteLine(coordinates[i].x);
            sw.WriteLine(coordinates[i].y);
            sw.WriteLine(coordinates[i].z);
        }
        
        sw.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        coordinates = new List<Vector3>();
        filename = "config.txt";
    }
}
