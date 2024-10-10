using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    public Transform keyHoleTransform;
    private GameObject key;
    public StorageVolume volume;
    public DoorManager door;

    public void KeyFound()
    {
        if ( door.requiresKey && !door.isOpen )
        {
            door.Unlock();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Key" )
        {
            key = other.gameObject;
            KeyFound();
            print("Rocky unlocked the door");
        }
    }
}
