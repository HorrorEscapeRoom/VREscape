using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManager : MonoBehaviour
{
    // Resource: https://blog.yarsalabs.com/basic-scene-manager-in-unity/

    // tl;dr how to use:
    // Add this script to whatever you want to use to switch scenes. for e.g A button
    // OnClick() -> SceneManager.ChangeScene
    // type in the scene name you want to change to

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
