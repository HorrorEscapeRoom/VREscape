// using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleManager : MonoBehaviour
{
    public GameObject door;
    public Transform knobPosition;
    public bool isOpen = false;

    // Update is called once per frame
    void Update()
    {
        if ( isOpen )
        {
            return;
        }
        /*
                Vector3 position = this.transform.InverseTransformDirection(knobPosition.position);
                position.y = 0;

                Vector3 targetPosition = this.transform.TransformPoint(position);
                // Vector3 current = this.transform.position;

                transform.LookAt(targetPosition, transform.up);*/
        if ( door.transform.eulerAngles.y >= 270 )
        {
            transform.LookAt(new Vector3(this.transform.position.x, knobPosition.position.y, knobPosition.position.z), transform.up);
        }
        else
        {
            transform.LookAt(new Vector3(knobPosition.position.x, knobPosition.position.y, this.transform.position.z), transform.up);
        }
            Debug.Log("DOOR - " + door.transform.eulerAngles.y);
        



        /*Vector3 current = this.transform.position;
        Quaternion lookAt = Quaternion.LookRotation(new Vector3(knobPosition.position.x, knobPosition.position.y, this.transform.position.z), transform.up);
        transform.localRotation = lookAt;*/

    }

}
