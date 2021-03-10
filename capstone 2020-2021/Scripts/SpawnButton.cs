using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public string filepath;


    private void getText()
    {
        filepath = GetComponent<UnityEngine.UI.Text>().text;
    }

    private void writeToConfigReader()
    {
        GameObject[] cr = GameObject.FindGameObjectsWithTag("Spawner");
        if (filepath != "")
        {
            cr[0].GetComponent<configReader>().readConfig(filepath);
        }
        else
        {
            cr[0].GetComponent<configReader>().readConfig();
        }
    }

    public void accessAndSend()
    {
        getText();
        writeToConfigReader();
    }

}