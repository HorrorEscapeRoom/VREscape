using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnobManager : MonoBehaviour
{
    DoorTrigger doorTrigger;
    XRKnob knob;
    public float openAngle;
    public float currentKnobvalue;

    private void Start()
    {
        currentKnobvalue = this.knob.value;
    }
    public void CheckDoorKnob(float knobValue)
    {
        if (knobValue < currentKnobvalue - openAngle || knobValue > currentKnobvalue + openAngle)
        {
            doorTrigger.OpenDoor();
        }
    }
}
