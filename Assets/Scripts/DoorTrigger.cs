using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    //[SerializeField] private bool openTrigger = false;

    public void OpenDoor()
    {
        //Add conditions to open door here e.g needs a key...

        myDoor.Play("DoorOpen", 0, 0.0f);
    }

}
