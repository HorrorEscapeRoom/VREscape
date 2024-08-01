using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTesting : MonoBehaviour
{
    [SerializeField] VRController controller;
    [SerializeField] bool startTeleport = false, endTeleport = false, cancelTeleport = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startTeleport){
            controller.InitiateTeleport(transform);
            startTeleport = false;
        }
        if(endTeleport){
            controller.Teleport();
            endTeleport = false;
        }
        if(cancelTeleport){
            controller.Teleport(true);
            cancelTeleport = false;
        }
    }
}
