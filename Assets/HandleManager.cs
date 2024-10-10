// using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleManager : MonoBehaviour
{
    public Transform knobPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 current = this.transform.position;
        transform.LookAt(new Vector3(knobPosition.position.x, knobPosition.position.y, this.transform.position.z), transform.up);
        

    }

}
