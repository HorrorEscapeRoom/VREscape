using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class XRItem : MonoBehaviour
{
    public float throwVelocity = 2.0f;
    public Vector3 holdOffset = Vector3.zero;
    public Vector3 holdRotation = Vector3.zero;
    void Start(){
        if(gameObject.tag != "XRItem"){
            Debug.LogError($"XRItem: {gameObject.name} does not have the tag XRItem. Please add the tag XRItem to the object.");
        }
        if(GetComponent<Collider>().isTrigger){
            Debug.LogError($"XRItem: {gameObject.name} is a trigger. Please set the collider to solid.");
        }
    }
}
