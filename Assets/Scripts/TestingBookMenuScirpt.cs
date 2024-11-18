using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestingBookMenuScirpt : MonoBehaviour
{
    public ObjectivesManager manager;
    public TextMeshProUGUI gui;
    // Start is called before the first frame update
    public void OnRemoveButton()
    {
        manager.RemoveObjective(01);
    }
    public void OnButtonClick()
    {
        // gui.text = "Hello, World!";
        manager.AddObjective(01, "Hello, World!", "This is an example objective");
        manager.ModifyObjective(01, "Hello, Rocky!", "blah, blah, blah");
        manager.AddObjective(02, "hello, World!", "This is an example objective");

        manager.AddObjective(03, "Hello1", "Blahm Blah, Blah");

        manager.RemoveObjective(02);


    }
}
