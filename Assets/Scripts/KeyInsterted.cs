using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInsterted : MonoBehaviour
{
    public KeyHolder key;

    public void OnItemPlaced(object heldItem)
    {
        StorageVolume volume = GetComponent<StorageVolume>();

        volume.SetLocks(false, false);

        key.KeyFound();
    }
}
