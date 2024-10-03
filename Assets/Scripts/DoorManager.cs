using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public DoorTrigger doorTrigger;
    public XRSlider doorknob;
    public HandleManager handle;

    public Transform newKnobTransform;

    public bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DoorKnobValue(float value)
    {
        if ( value >= 0.6f && !isOpen )
        {
            isOpen = true;

            doorTrigger.OpenDoor();

            doorknob.gameObject.SetActive(false);

            handle.gameObject.transform.rotation.Euler(0f, -86.55f, -180));
        }

        print(value.ToString());
    }
}
