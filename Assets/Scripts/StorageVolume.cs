using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StorageVolume : MonoBehaviour
{
    Transform heldItem;
    bool placeLocked = false, pickupLocked = false;

    void Start(){
        if(gameObject.tag != "StorageVolume"){
            Debug.LogError($"StorageVolume: {gameObject.name} does not have the tag StorageVolume. Please add the tag StorageVolume to the object.");
        }
        if(!GetComponent<Collider>().isTrigger){
            Debug.LogError($"StorageVolume: {gameObject.name} is not a trigger. Please set the collider to be a trigger.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(heldItem != null){
            heldItem.position = transform.position;
            heldItem.rotation = transform.rotation;
        }
    }
    public void SetLocks(bool canPlace, bool canPickup){
        placeLocked = canPlace;
        pickupLocked = canPickup;
    }
    public bool CanPlace(){ return !placeLocked && heldItem == null; }
    public bool CanPickup(){ return !pickupLocked && heldItem != null; }
    public void SetItem(Transform item){ 
        heldItem = item;
        heldItem.GetComponent<Collider>().enabled = false;
    }
    public Transform GetItem(){
        Transform item = heldItem;
        item.GetComponent<Collider>().enabled = true;
        heldItem = null;
        return item; 
    }
}
