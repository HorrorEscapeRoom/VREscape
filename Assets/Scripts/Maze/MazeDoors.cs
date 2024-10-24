using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoors : MonoBehaviour
{
    [SerializeField] GameObject LDoor, RDoor;
    [SerializeField] AudioSource LDoorAudio, RDoorAudio;
    bool LDoorOpen, RDoorOpen;
    public void OpenLDoor()
    {
        if (!LDoorOpen)
        {
            LDoorOpen = true;
            Destroy(LDoor);
            LDoorAudio.Play();
        }
    }
    public void OpenRDoor()
    {
        if (!RDoorOpen)
        {
            RDoorOpen = true;
            Destroy(RDoor);
            RDoorAudio.Play();
        }
    }
}
