using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestingBookMenuScirpt : MonoBehaviour
{
    public ObjectivesManager manager;
    public TextMeshProUGUI gui;
    // Start is called before the first frame update
    public void OnButtonClick()
    {
        gui.text = "Hello, World!";
        manager.AddObjective(01, "Hello, World!", "This is an example objective");
    }
}
