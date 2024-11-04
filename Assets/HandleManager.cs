// using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleManager : MonoBehaviour
{
    public Transform knobPosition;
    public bool isOpen = false;

    // Update is called once per frame
    void Update()
    {
        if ( isOpen )
        {
            return;
        }
        Vector3 current = this.transform.position;
        transform.LookAt(new Vector3(knobPosition.position.x, knobPosition.position.y, this.transform.position.z), transform.up);
        

    }

}
