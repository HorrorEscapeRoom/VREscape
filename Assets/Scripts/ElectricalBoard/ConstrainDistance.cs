using System.Threading;
using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class ConstrainDistance : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public float maxDistance = 0.2f;
    public float forceStrength = 10f;

    private LineRenderer theline;

    private Rigidbody rb1;
    private Rigidbody rb2;

    GameObject LeftController;  
    GameObject RightController;

    private bool isDropped = false;
    private float savedTime;



    void Start()
    {
        if (object1 == null || object2 == null)
        {
            Debug.LogError("Object1 or Object2 is not assigned.");
            enabled = false; // Disable the script if objects are not assigned
            return;
        }

        rb1 = object1.GetComponent<Rigidbody>();
        rb2 = object2.GetComponent<Rigidbody>();

        if (rb1 == null || rb2 == null)
        {
            Debug.LogError("Rigidbody components are missing from Object1 or Object2.");
            enabled = false; // Disable the script if Rigidbody components are missing
            return;
        }
        theline = object1.GetComponent<LineRenderer>();
        if (theline == null)
        {
            theline = object2.GetComponent<LineRenderer>();
            if (theline == null)
            {
                Debug.LogError("LineRenderer is not assigned.");
                enabled = false; // Disable the script if LineRenderer is not assigned
            }
        }
        LeftController = GameObject.Find("XRControllerLeft");
        RightController = GameObject.Find("XRControllerRight");

    }


    void FixedUpdate()
    {
        if (rb1 == null || rb2 == null || theline == null) return;

        var leftHandObject = LeftController.GetComponent<HandItemTracker>();
        var righthandObject = RightController.GetComponent<HandItemTracker>();

        Vector3 direction = object2.transform.position - object1.transform.position;
        float distance = direction.magnitude;
        Vector3[] positions = new Vector3[2]
        {
            object1.transform.position,
            object2.transform.position
        };

        theline.SetPositions(positions);

        if (distance > maxDistance)
        {
            if(savedTime < Time.time - 0.5f)
            {
            savedTime = Time.time;
            isDropped = true;           
            }
            Vector3 forceDirection = direction.normalized;
            Vector3 force = forceDirection * forceStrength * (distance - maxDistance);

            rb1.AddForce(force, ForceMode.Impulse);
            rb2.AddForce(-force, ForceMode.Impulse);
            if(!rb1.isKinematic)
            {
            rb1.velocity = new Vector3(0, 0, 0);
            }
            if(!rb2.isKinematic)
            {
            rb2.velocity = new Vector3(0, 0, 0);
            }


            if (distance > (0.51f) && isDropped == true)
            {
                if (leftHandObject.heldItem != null && righthandObject.heldItem == null)
                {
                    Debug.Log("left not null");
                    leftHandObject.DropThatShit();
                    isDropped = false;
                } else if (righthandObject.heldItem != null && leftHandObject.heldItem == null)
                {
                    isDropped = false;
                    righthandObject.DropThatShit();
                } else if (leftHandObject.heldItem != null && righthandObject != null)
                {
                    isDropped = false;
                    leftHandObject.DropThatShit();
                }
            }



        }
    }

    //    ADD THIS TO HANDITEMTRACKER.CS
    //    
    //    make public touchingobject and heldItem
    //    
    //
    //    IEnumerator PerformActionsWithDelay(GameObject droppedObject)
    //    {
    //
    //        // Wait for 1 seconds
    //        yield return new WaitForSeconds(1f);
    //        droppedObject.tag = "XRItem";
    //    }
    //
    //    public void DropThatShit()
    //    {
    //        GameObject theObject = heldItem.gameObject;
    //        theObject.tag = "Untagged";
    //        theObject.GetComponent<Rigidbody>().useGravity = true;
    //        theObject.GetComponent<Rigidbody>().isKinematic = false;
    //        theObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    //        touchingObj.Remove(heldItem.GetComponent<Collider>());
    //        holdID = HoldType.None;
    //        heldItem = null;
    //        StartCoroutine(PerformActionsWithDelay(theObject));
    //    }

}
