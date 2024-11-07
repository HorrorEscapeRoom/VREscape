using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Door : MonoBehaviour
{
    GameObject door;
    bool opened;
    bool doorOpenable;
    float initPosition;
    bool lockDial;
    int maxDoorOpenAngle = 180;

    enum Sound
    {
        Moving,
        Locking,
        Unlocking
    }

    public bool CheckLock() => door.transform.rotation.y == initPosition && opened;

    public float InitialPosition
    {
        get => initPosition;
    }

    public bool Openable
    {
        get => doorOpenable;
        set
        {
            if (doorOpenable != value)
            {
                doorOpenable = value;
            }
        }
    }

    public bool Opened
    {
        get => opened;
        set
        {
            if (opened != value)
            {
                opened = value;
            }
        }
    }

    //public bool LockDial
    //{
    //    get => lockDial;
    //    set
    //    {
    //        if (lockDial != value)
    //        {
    //            lockDial = value;
    //        }
    //    }
    //}

    void Rotate()
    {

    }

    public void HandleDoorRotation()
    {
        float currentYRotation = door.transform.rotation.y;
        float diffInitCurrentYRotation = currentYRotation - initPosition;
        if (currentYRotation == initPosition && opened)
        {
            PlaySound(Sound.Locking);  // # door
            doorOpenable = false;  // # door
            Debug.Log("Door opened");
        }
        else
        {
            if (diffInitCurrentYRotation > 0 && diffInitCurrentYRotation < 180 || diffInitCurrentYRotation < 0)
            {
                PlaySound(Sound.DoorMoving);  // # door
                Debug.Log("Handling door rotation.");
            }
        }
    }


    bool UpdateDoorState()
    {
        opened = door.transform.eulerAngles.z > 0;
        lockDial = !opened;
        Debug.Log($"Door state updated: DoorOpened={opened}, LockDial={lockDial}");
        return opened;
    }

    // Start is called before the first frame update
    void Start()
    {
        // door = transform.parent.gameObject;
        initPosition = door.transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
