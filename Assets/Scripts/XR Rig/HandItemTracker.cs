using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItemTracker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Hand hand;
    List<Collider> touchingObj = new List<Collider>(),
    touchingContactPoints = new List<Collider>(),
    touchingControls = new List<Collider>();
    HoldType holdID = HoldType.None;
    Transform heldItem;
    IInput input;
    Vector3 lastPos;
    VRHudManager hud;
    VRController controller;
    GameObject ControlHost;
    List<Vector3> velocitySamples = new List<Vector3>();

    //Only in this project
    float snapCooldown = 0;
    bool teleporting = false;
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
            if(input.RController.Grip.ReadValue<float>() > 0.5f){ PickupLogic(); }
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
            if(stick.x > 0.5f && snapCooldown <= 0){
                controller.SnapTurn(45);
                snapCooldown = 1f;
            }else if(stick.x < -0.5f && snapCooldown <= 0){
                controller.SnapTurn(-45);
                snapCooldown = 1f;
            }
        }
        snapCooldown -= Time.deltaTime;
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
        
        try{
            string touching = $"Touching: {holdID}";
            if(holdID == HoldType.Item){
                touching += $" {heldItem.name}";
            }
            if(holdID == HoldType.Control){
                touching += $" {ControlHost.name}";
            }
            foreach(Collider col in touchingObj){
                touching += $"\n{col.name}";
            }
            foreach(Collider col in touchingContactPoints){
                touching += $"\n{col.name}";
            }
            hud.SetTouchingText(hand == Hand.Right, touching);
        }catch (System.Exception e){
            touchingObj.Clear();
            touchingControls.Clear();
            hud.Debug(e.Message);
        }
    }
    void FixedUpdate(){
        Vector3 vel = (transform.parent.parent.localPosition - lastPos) / Time.deltaTime * 1.1f;
        //rotate vel 90 degrees to the right
        vel = new Vector3(-vel.z, vel.y, vel.x);
        velocitySamples.Add(vel);
        lastPos = transform.parent.parent.localPosition;
        if(velocitySamples.Count > 10){
            velocitySamples.RemoveAt(0);
        }
    }
    void PickupLogic(){
        if(holdID == HoldType.None && touchingObj.Count + touchingContactPoints.Count + touchingControls.Count > 0){
            foreach(Collider inv in touchingContactPoints){
                StorageVolume storage = inv.GetComponent<StorageVolume>();
                if(storage.CanPickup()){
                    holdID = HoldType.Item;
                    heldItem = storage.GetItem();
                    heldItem.BroadcastMessage("OnPickup", SendMessageOptions.DontRequireReceiver);
                    return;
                }
            }
            foreach(Collider col in touchingObj){
                holdID = HoldType.Item;
                heldItem = col.transform;
                heldItem.GetComponent<Rigidbody>().isKinematic = true;
                heldItem.BroadcastMessage("OnPickup", SendMessageOptions.DontRequireReceiver);
                return;
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
        }else if(other.gameObject.CompareTag("ContactPoint")){
            touchingContactPoints.Add(other);
        }else if(other.gameObject.CompareTag("XRControl")){
            touchingControls.Add(other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        //if its a XRItem or ContactPoint then remove it from the list
        if (other.gameObject.CompareTag("XRItem")){
            touchingObj.Remove(other);
        } else if(other.gameObject.CompareTag("ContactPoint")){
            touchingContactPoints.Remove(other);
        } else if(other.gameObject.CompareTag("XRControl")){
            touchingControls.Remove(other);
        }
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