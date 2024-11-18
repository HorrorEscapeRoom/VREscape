using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInsterted : MonoBehaviour
{
    public KeyHolder key;

    public void OnItemPlaced(object heldItem)
    {
        Transform item = (Transform)heldItem;

        if ( item.gameObject.transform.GetChild(0).tag == "Key" )
        {
            StorageVolume volume = GetComponent<StorageVolume>();

            volume.SetLocks(false, false);

            key.KeyFound();
        }
       
    }
}
