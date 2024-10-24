using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimickRotation : MonoBehaviour
{
    public GameObject otherObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform newRot = transform;
        newRot.rotation = Quaternion.Euler(transform.eulerAngles.x,0,0);
        otherObject.transform.SetPositionAndRotation(otherObject.transform.position, newRot.rotation);
        Vector3 test = new Vector3(otherObject.transform.position.x+0.25f, transform.position.y, transform.position.z);
        transform.SetPositionAndRotation(test, transform.rotation);
    }
}
