using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemTracker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Hand hand;
    HoldType holdID = HoldType.None;
    Transform heldItem;
    IInput input;
    Vector3 lastPos;
    VRController controller;
    GameObject ControlHost;
    List<Vector3> velocitySamples = new List<Vector3>();

    //Only in this project
    bool teleporting = false, canSnapTurn = true;
    void Start()
    {
        input = new IInput();
        input.Enable();
        controller = GetComponentInParent<VRController>();
    }

    void LateUpdate() {

    }
    // Update is called once per frame
    void Update()
    {
        if(hand == Hand.Left){
            if(input.LController.Grip.ReadValue<float>() > 0.5f){ PickupLogic(); } //this has code smell but i cant be damned fixing it rn
            else if(holdID != HoldType.None){ DropLogic(); }
            float trigger = input.LController.Trigger.ReadValue<float>();
            if(trigger > 0.1f && holdID == HoldType.Item){
                heldItem.BroadcastMessage("Use", trigger, SendMessageOptions.DontRequireReceiver);
            }

            Vector2 stick = input.LController.Stick.ReadValue<Vector2>();
            controller.Move(stick);

            if(input.LController.SecondayButton.triggered){ controller.LogOffsetFromHead(transform.position); }
        }
        else{
            if(input.RController.Grip.ReadValue<float>() > 0.5f){ PickupLogic(); } //this has code smell but i cant be damned fixing it rn
            else if(holdID != HoldType.None){ DropLogic(); }
            float trigger = input.RController.Trigger.ReadValue<float>();
            if(trigger > 0.1f && holdID == HoldType.Item){ 
                heldItem.BroadcastMessage("Use", trigger, SendMessageOptions.DontRequireReceiver);
            }

            if(input.RController.PrimaryButton.triggered){ controller.Jump(); } //implemented from my game, we probably dont need this

            Vector2 stick = input.RController.Stick.ReadValue<Vector2>();
            if(stick.y > 0.5f){
                controller.InitiateTeleport(transform);
                teleporting = true;
            }else if(teleporting){
                controller.Teleport();
                teleporting = false;
            }
            if(stick.x > 0.5f && canSnapTurn){
                controller.SnapTurn(45);
                canSnapTurn = false;
            }else if(stick.x < -0.5f && canSnapTurn){
                controller.SnapTurn(-45);
                canSnapTurn = false;
            } else if(stick.x == 0){
                canSnapTurn = true;
            }
        }
        if(holdID == HoldType.Item){
            Vector3 offset = Vector3.zero;
            Quaternion rotationOffset = Quaternion.identity;
            if(heldItem.TryGetComponent(out XRItem item)){
                offset = item.holdOffset;
                rotationOffset = Quaternion.Euler(item.holdRotation);
            }
            heldItem.position = transform.position + transform.TransformDirection(offset);
            heldItem.rotation = transform.rotation * rotationOffset;
        }
        
    }
    void FixedUpdate(){
        Vector3 vel = (transform.parent.localPosition - lastPos) / Time.deltaTime * 1.1f;
        //rotate vel 90 degrees to the right
        velocitySamples.Add(vel);
        lastPos = transform.parent.localPosition;
        if(velocitySamples.Count > 10){
            velocitySamples.RemoveAt(0);
        }
    }
    void PickupLogic(){
        Vector3 handCheck = transform.position + (transform.forward * -0.03f);
        Collider[] touchingObj = Physics.OverlapSphere(handCheck, 0.035f);
        List<Collider> touchingControls = new List<Collider>();
        List<Collider> touchingContactPoints = new List<Collider>();
        foreach(Collider col in touchingObj){
            if(col.gameObject.CompareTag("XRControl")){
                touchingControls.Add(col);
            }
            else if(col.gameObject.CompareTag("ContactPoint")){
                touchingContactPoints.Add(col);
            }
        }
        if(holdID == HoldType.None && touchingObj.Length > 0){
            foreach(Collider inv in touchingContactPoints){
                StorageVolume storage = inv.GetComponent<StorageVolume>();
                if(storage.CanPickup()){
                    holdID = HoldType.Item;
                    heldItem = storage.GetItem();
                    heldItem.BroadcastMessage("OnPickup", transform, SendMessageOptions.DontRequireReceiver);
                    heldItem.GetComponent<Collider>().enabled = false;
                    //hud.Debug($"{hand} hand picked up item ({heldItem.name}) from storage {inv.name}");
                    return;
                }
            }
            foreach(Collider col in touchingObj){
                if(col.gameObject.CompareTag("XRItem")){ 
                    holdID = HoldType.Item;
                    heldItem = col.transform;
                    heldItem.GetComponent<Rigidbody>().isKinematic = true;
                    heldItem.BroadcastMessage("OnPickup", transform, SendMessageOptions.DontRequireReceiver);
                    heldItem.GetComponent<Collider>().enabled = false;
                    //hud.Debug($"{hand} hand picked up item ({heldItem.name})");
                    return;
                }
            }
            if(touchingControls.Count > 0){
                ControlHost = touchingControls[0].transform.gameObject;
                ControlHost.BroadcastMessage("Grabbed", transform, SendMessageOptions.DontRequireReceiver);
                holdID = HoldType.Control;
                return;
                //hud.Debug($"{hand} hand grabbed control {ControlHost.name}");
            }
        }
    }
    void DropLogic(){
        if(holdID != HoldType.None){
            holdID = HoldType.None;
            if(heldItem == null){
                //we hopefully grabbed a control
                if(ControlHost != null){
                    ControlHost.BroadcastMessage("Released", SendMessageOptions.DontRequireReceiver);
                    ControlHost = null;
                    //hud.Debug($"{hand} hand released control");
                    return;
                }
                else{
                    Debug.LogError("Error: expected control but none found");
                }
            }
            Vector3 handCheck = transform.position + (transform.forward * -0.03f);
            Collider[] col = Physics.OverlapSphere(handCheck, 0.035f);
            //if we are touching a storage volume then add the item to it
            foreach(Collider inv in col){
                StorageVolume storage;
                if(inv.TryGetComponent(out storage) && storage.CanPlace()){
                    heldItem.GetComponent<Collider>().enabled = true;
                    storage.SetItem(heldItem);
                    heldItem.BroadcastMessage("OnDrop", SendMessageOptions.DontRequireReceiver);
                    heldItem = null;
                    //hud.Debug($"{hand} hand placed item in storage {inv.name}");
                    return;
                }
            }
            heldItem.GetComponent<Collider>().enabled = true;
            Rigidbody heldRb = heldItem.GetComponent<Rigidbody>();
            heldRb.isKinematic = false;
            Vector3 vel = GetVelocity(10);
            //Leaving this here in case we need to add a power throw mechanic later
            // if(heldItem.TryGetComponent(out ThrowVelocityMod bonus)){ 
            //     //hud.Debug($"hand vel {vel.magnitude}");
            //     if(vel.magnitude > bonus.powerThreshhold){ vel *= bonus.throwVelocity; }
            // }
            heldRb.velocity = vel;
            heldItem.BroadcastMessage("OnDrop", SendMessageOptions.DontRequireReceiver);
            //hud.Debug($"{hand} hand dropped item ({heldItem.name})");
            heldItem = null;
            //clear any duplicates from touching items
        }
    }
    public Vector3 GetVelocity(int samples = 2){
        Vector3 avg = Vector3.zero;
        for(int i = 0; i < samples; i++){
            avg += velocitySamples[i];
        }
        return avg / samples;
    }
}
[System.Serializable]
enum Hand
{
    Left,
    Right
}
enum HoldType{
    None, Item, Control
}