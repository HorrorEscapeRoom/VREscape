using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorManager : MonoBehaviour
{
    public UnityEvent OnDoorOpened;

    // Unity set in unity if true the door will require an
    // key to be opened otherwise of false will open without an key
    public bool requiresKey;

    public DoorTrigger doorTrigger;
    public XRSlider doorknob;
    public HandleManager handle;

    public Transform newKnobTransform;

    public bool isOpen = false;

    public void Unlock()
    {
        requiresKey = false;
        doorknob.gameObject.SetActive(true);
    }

    public void ExitGameOnOpen()
    {
        print("ExitGame");
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        if ( requiresKey )
        {
            doorknob.gameObject.SetActive(false);
        }
    }

    public void DoorKnobValue(float value)
    {
        if ( requiresKey )
        {
            return;
        }
        
        if ( value >= 0.6f && !isOpen )
        {

            OnDoorOpened.Invoke();

            isOpen = true;

            doorTrigger.OpenDoor();

            
            handle.isOpen = true;

            handle.transform.Rotate(0, 55, 0);
        }

        print(value.ToString());
    }
}
