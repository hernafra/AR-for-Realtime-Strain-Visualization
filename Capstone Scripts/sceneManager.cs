using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class sceneManager : MonoBehaviour
{
    public string destination;

    public void moveToScene()
    {
        SceneManager.LoadSceneAsync(sceneName: destination);
    }


}
