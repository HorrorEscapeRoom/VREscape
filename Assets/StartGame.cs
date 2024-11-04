using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public SceneManager sceneManager;
    public string sceneNameToLoad;

    void OnPickup()
    {
        print("HELLO, ROCKY!");
        sceneManager.ChangeScene(sceneNameToLoad);
    }
}
