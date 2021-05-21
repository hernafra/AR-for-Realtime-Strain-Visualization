using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class csvreader : MonoBehaviour
{
    public TextAsset csvFile;
    public List<StrainSnapshot> StrainData = new List<StrainSnapshot>();

    // Holds the initial time in the data file
    // Without this, nothing would happen in the scene until 28 mins
    public float timeoffset;
    // The minimum value shown in the file
    public float floor;
    // Float used to resize displacement values
    public float scale;
    // Index into StrainData that holds where we are in time
    public int index;
    // List of each gauge's initial value
    public List<float> zero;
    public float maxval;
    public float minval;


    // Start is called before the first frame update
    public void Start()
    {
        index = 0;

        // -1 serves as flags so the reader knows these values are
        // uninitialized as opposed to zeroed
        timeoffset = -1;
        floor = -1;
        scale = -1;
        ReadCSV();
        Debug.Log(StrainData.Count);
    }

    // Called at each frame
    // Keeps an Index field that is used to select a row from StrainData
    // Assuming all entries in StrainData are ordered by ascending time,
    // Update() holds onto whichever entry it needs to parse next and only sends
    // the displace signal to the gauges when "ms" exceeds "curTime"
    public void Update()
    {
        // Only runs if there are more data lines to read
        if (index < StrainData.Count) { 
            // Gets the time and adds the zeroed offset (first entry's time)
            float ms = 5 * (Time.time * 1000.0F) + timeoffset;

            // Gets the "time"entry of the next table entry to be parsed
            float curTime = StrainData[index].Time;
            if (ms > curTime)
            {
                signalGauges(StrainData[index]);
                index++;
            }
        }
    }

    // Signals all gauges to displace by a given value
    // Takes in a StrainSnapshot with the slip values that are passsed
    //  note: doesn't displace each gauge BY entry.value. It tells the gauges
    //  what their current strain value is and the GAUGES need to determine how
    //  much to move
    public void signalGauges(StrainSnapshot entry)
    {
        // Grabs all gauge objects in the environment
        gaugeData[] gauges = GameObject.FindObjectsOfType<gaugeData>();

        // Grabs the strain data from the passed entry
        // Rescales the value so that it isn't too large/small of a displacement
        // in the actual scene
        float v1 = entry.WSlip1 * scale - floor;
        float v2 = entry.WSlip2 * scale - floor;
        float v3 = entry.WSlip3 * scale - floor;
        float v4 = entry.WSlip4 * scale - floor;
        float v5 = entry.ESlip5 * scale - floor;
        float v6 = entry.ESlip6 * scale - floor;
        float v7 = entry.ESlip7 * scale - floor;
        float v8 = entry.ESlip8 * scale - floor;

        // Calls modify (displace/recolor) for each gauge
        gauges[0].modify(v1);
        gauges[1].modify(v2);
        gauges[2].modify(v3);
        gauges[3].modify(v4);
        gauges[4].modify(v5);
        gauges[5].modify(v6);
        gauges[6].modify(v7);
        gauges[7].modify(v8);
    }

    protected virtual bool isFileLocked(FileInfo fi)
    {
        try
        {
            using (FileStream stream = fi.Open(FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (IOException)
        {
            // File is locked, try again later
            Debug.Log("File is Locked and cannot be read");
            return true;
        }

        // File isn't locked, good to go
        Debug.Log("File is not locked and available for Read");
        return false;
    }

    // Opens the StrainData.csv file located in Assets/Resources and loads
    // the info into StrainData. Also sets the scale, min values, and zeroed
    // values for each gauge
    public void ReadCSV()
    {
        // Reads the StrainData.csv file. Function includes "./Assets/Resources" as implicit prefix
        // and ignores the file extension
        var csvFile = Resources.Load<TextAsset>("StrainData");

        // Converts csvFile into a string array delimited by newlines
        var lines = csvFile.ToString().Split('\n');

        float prevTime = 0;
        float offset = 0;
        float min = -1;
        float max = -1;

        // For each entry in the file
        for (int j = 7; j < lines.Length - 1; j++) {

            // Splits the .csv file values and puts them into an array (holds strings, not floats, ints, etc.)
            var values = lines[j].Split(',');

            // values[0] is of format Min:Sec.X
            // Splits values[0] into { Min , Sec.X }
            var timeValue = (values[0].ToString()).Split(':');

            // Converts timeValue into milliseconds
            // float.Parse(x) converts string x into float value
            float timeFloat = (float.Parse(timeValue[0]) * 60 + float.Parse(timeValue[1])) * 1000;

            // Only called when timeoffset == -1
            if (timeoffset < 0)
            {
                // Assigns the offset to timefloat
                // note: timefloat should be the FIRST entry's time now
                timeoffset = timeFloat;

                // Fill out the zero table (the initial values of each gauge)
                // In the data file provided, initial values ranged 0.2-1.3 while only going at most
                // .4 up/down. This makes it so the min-max range is artificially enlarged. When scaled
                // down for a specific size, the .4 units of movement are relatively smaller.
                // Zeroing makes is to the min-max range is smaller, meaning movements are more pronounced
                // and easy to see
                for (int i = 8; i < 16; i++)
                {
                    zero.Add(float.Parse(values[i]));
                }
            }

            // Used to handle duplicate time entries
            if (prevTime == timeFloat)
            {
                // For each duplicate, add 50ms to the time
                offset += 50;
                timeFloat += offset;
            }
            else
            {
                // When the current entry is DIFFERENT than the last,
                // reset the offset and set the new prevTime for future checks
                prevTime = timeFloat;
                offset = 0;
            }

            // Create a new entry for the table
            StrainSnapshot newEntry = new StrainSnapshot()
            {
                // Alternative Float Time value
                // Time= TimeInSeconds,
                Time = timeFloat,
                KipsLoad = float.Parse(values[1]),
                ActDispIn = float.Parse(values[2]),
                SCLDisp = float.Parse(values[3]),
                WQuarter = float.Parse(values[4]),
                EQuarter = float.Parse(values[5]),
                CompStrain = float.Parse(values[6]),
                TenStrain = float.Parse(values[7]),
                WSlip1 = float.Parse(values[8]) - zero[0],
                WSlip2 = float.Parse(values[9]) - zero[1],
                WSlip3 = float.Parse(values[10]) - zero[2],
                WSlip4 = float.Parse(values[11]) - zero[3],
                ESlip5 = float.Parse(values[12]) - zero[4],
                ESlip6 = float.Parse(values[13]) - zero[5],
                ESlip7 = float.Parse(values[14]) - zero[6],
                ESlip8 = float.Parse(values[15]) - zero[7],
                WVertSep = float.Parse(values[16]),
                EVertSep = float.Parse(values[17]),
                NCLDisp = float.Parse(values[18]),
                WSupport = float.Parse(values[19]),
                ESupport = float.Parse(values[20])
            };

            // Check for new min/maxes
            // Only checks for the W/ESlipN values
            for (int i = 8; i < 16; i++)
            {
                if (min < 0 || float.Parse(values[i]) - zero[i - 8] < min)
                {
                    min = float.Parse(values[i]) - zero[i - 8];
                }
                else if (float.Parse(values[i]) - zero[i - 8] > max)
                {
                    max = float.Parse(values[i]) - zero[i - 8];
                }
            }
            minval = min;
            maxval = max;
            StrainData.Add(newEntry);
        }
        floor = min;

        // Sets what value to scale each value by when passing them
        // to strain gauges.
        // 0.5F is the MAXIMUM displacement
        scale = 0.5F / (max - min);
    }


}

// StrainSnapshot is a custom object that simply keeps track of a dta row from the StrainData.csv file
// I was unsure whether simple arrays or a list structure would be preferable but I settled on a list due to the nice features C# List<T> provides
// and iterating over them is still rather easy using loops like in my above debug sample.
//
// Time is currently stored as a string due to issues with the way DASY Lab stores the time variable.
// If you want Time in an more usable format like float or double I've included a commented out method to convert it to a float value representing it in seconds in the above code
// You will just need to comment out the 'public String Time { get; set; }
[System.Serializable]
public class StrainSnapshot
{
    // public float    Time           { get; set; }
    public float   Time        { get; set; }
    public float    KipsLoad    { get; set; }
    public float    ActDispIn   { get; set; }
    public float    SCLDisp     { get; set; }
    public float    WQuarter    { get; set; }
    public float    EQuarter    { get; set; }
    public float    CompStrain  { get; set; }
    public float    TenStrain   { get; set; }
    public float    WSlip1      { get; set; }
    public float    WSlip2      { get; set; }
    public float    WSlip3      { get; set; }
    public float    WSlip4      { get; set; }
    public float    ESlip5      { get; set; }
    public float    ESlip6      { get; set; }
    public float    ESlip7      { get; set; }
    public float    ESlip8      { get; set; }
    public float    WVertSep    { get; set; }
    public float    EVertSep    { get; set; }
    public float    NCLDisp     { get; set; }
    public float    WSupport    { get; set; }
    public float    ESupport    { get; set; }
}
