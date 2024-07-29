using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemTracker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Hand hand;
    List<Collider> touchingObj = new List<Collider>(), touchingContactPoints = new List<Collider>();
    bool holdingItem = false;
    Transform heldItem;
    IInput input;
    Vector3 lastPos;
    VRHudManager hud;
    VRController controller;
    List<Vector3> velocitySamples = new List<Vector3>();

    void Start()
    {
        input = new IInput();
        input.Enable();
        hud = FindObjectOfType<VRHudManager>();
        controller = GetComponentInParent<VRController>();
    }

    void LateUpdate() {

    }
    // Update is called once per frame
    void Update()
    {
        if(hand == Hand.Left){
            if(input.LController.Grip.ReadValue<float>() > 0.5f){ PickupLogic(); }
            else if(holdingItem){ DropLogic(); }

            Vector2 stick = input.LController.Stick.ReadValue<Vector2>();
            controller.Move(stick);

            if(input.LController.SecondayButton.triggered){ controller.LogOffsetFromHead(transform.position); }
            
        }
        else{
            if(input.RController.Grip.ReadValue<float>() > 0.5f){ PickupLogic(); }
            else if(holdingItem){ DropLogic(); }

            if(input.RController.PrimaryButton.triggered){ controller.Jump(); }
        }

        if(holdingItem){
            Vector3 offset = Vector3.zero;
            Quaternion rotationOffset = Quaternion.identity;
            if(heldItem.TryGetComponent(out XRItem item)){
                offset = item.holdOffset;
                rotationOffset = Quaternion.Euler(item.holdRotation);
            }
            heldItem.position = transform.position + transform.TransformDirection(offset);
            heldItem.rotation = transform.rotation * rotationOffset;
        }
        lastPos = transform.position;

        
        string touching = $"{holdingItem}";
        if(holdingItem){ touching += $" {heldItem.name}"; }
        foreach(Collider col in touchingObj){ touching += $"\n{col.name}"; }
        foreach(Collider col in touchingContactPoints){ touching += $"\n{col.name}"; }
        hud.SetTouchingText(hand == Hand.Right, touching);
    }
    void FixedUpdate(){
        velocitySamples.Add(((transform.position - lastPos) / Time.deltaTime) * 1.1f);
        lastPos = transform.position;
        if(velocitySamples.Count > 10){
            velocitySamples.RemoveAt(0);
        }
    }
    void PickupLogic(){
        if(touchingObj.Count + touchingContactPoints.Count > 0 && !holdingItem){
            foreach(Collider inv in touchingContactPoints){
                StorageVolume storage = inv.GetComponent<StorageVolume>();
                if(storage.CanPickup()){
                    holdingItem = true;
                    heldItem = storage.GetItem();
                    heldItem.BroadcastMessage("OnPickup", SendMessageOptions.DontRequireReceiver);
                    return;
                }
            }
            foreach(Collider col in touchingObj){
                holdingItem = true;
                heldItem = col.transform;
                heldItem.GetComponent<Rigidbody>().isKinematic = true;
                heldItem.BroadcastMessage("OnPickup", SendMessageOptions.DontRequireReceiver);
                return;
            }
        }
    }
    void DropLogic(){
        if(holdingItem){
            holdingItem = false;
            //if we are touching a storage volume then add the item to it
            foreach(Collider inv in touchingContactPoints){
                StorageVolume storage = inv.GetComponent<StorageVolume>();
                if(storage.CanPlace()){
                    storage.SetItem(heldItem);
                    touchingObj.Remove(heldItem.GetComponent<Collider>());
                    heldItem.BroadcastMessage("OnDrop", SendMessageOptions.DontRequireReceiver);
                    heldItem = null;
                    return;
                }
            }
            Rigidbody heldRb = heldItem.GetComponent<Rigidbody>();
            heldRb.isKinematic = false;
            Vector3 vel = GetVelocity(10);
            if(heldItem.TryGetComponent(out XRItem bonus)){ vel *= bonus.throwVelocity; }
            heldRb.velocity = vel;
            heldItem.BroadcastMessage("OnDrop", SendMessageOptions.DontRequireReceiver);
            heldItem = null;
        }
    }
    public Vector3 GetVelocity(int samples = 2){
        Vector3 avg = Vector3.zero;
        for(int i = 0; i < samples; i++){
            avg += velocitySamples[i];
        }
        return avg / samples;
    }
    void OnTriggerEnter(Collider other)
    {
        //if its a XRItem or ContactPoint then add it to the list
        if (other.gameObject.CompareTag("XRItem")){
            touchingObj.Add(other);
        }else if(other.gameObject.CompareTag("StorageVolume")){
            touchingContactPoints.Add(other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        //if its a XRItem or ContactPoint then remove it from the list
        if (other.gameObject.CompareTag("XRItem")){
            touchingObj.Remove(other);
        } else if(other.gameObject.CompareTag("StorageVolume")){
            touchingContactPoints.Remove(other);
        }
    }
}
[System.Serializable]
enum Hand
{
    Left,
    Right
}
