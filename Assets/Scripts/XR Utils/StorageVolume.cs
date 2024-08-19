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
    /// <summary>
    /// Locks the volume from placing and picking up items.
    /// </summary>
    public void SetLocks(bool canPlace, bool canPickup){
        placeLocked = canPlace;
        pickupLocked = canPickup;
    }
    /// <summary>
    /// Returns true if the volume is not locked and there is no item in the volume.
    /// </summary>
    public bool CanPlace(){ return !placeLocked && heldItem == null; }
    /// <summary>   
    /// Returns true if the volume is not locked and there is an item in the volume.
    /// </summary>
    public bool CanPickup(){ return !pickupLocked && heldItem != null; }
    /// <summary>
    /// Sets the item in the volume to the item passed in.
    /// </summary>
    public void SetItem(Transform item){ 
        if(heldItem != null){
            Debug.LogWarning($"StorageVolume: {gameObject.name} already has an item in it. Ensure that the volume is empty before setting an item.");
            return;
        }
        heldItem = item;
        heldItem.GetComponent<Collider>().enabled = false;
        gameObject.BroadcastMessage("OnItemPlaced", heldItem, SendMessageOptions.DontRequireReceiver);
    }
    /// <summary>
    /// [Depricated] Returns the item that is currently being held by the storage volume.   you should cache the item when it is placed instead of reading it from the storage volume.
    /// </summary>
    public GameObject ReadItem(){
        return heldItem.gameObject;
    }
    /// <summary>
    /// Returns the item in the volume and sets the item in the volume to null.
    /// </summary>
    public Transform GetItem(){
        Transform item = heldItem;
        item.GetComponent<Collider>().enabled = true;
        gameObject.BroadcastMessage("OnItemPickedUp", heldItem, SendMessageOptions.DontRequireReceiver);
        heldItem = null;
        return item; 
    }
}
