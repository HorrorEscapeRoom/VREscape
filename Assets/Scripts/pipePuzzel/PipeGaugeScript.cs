using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGaugeScript : MonoBehaviour
{

	public Transform Needle;
	public void UpdateGauge(float presher)
	{
		
		Needle.localRotation = Quaternion.Euler(Needle.localRotation.x, Needle.localRotation.y, Mathf.LerpAngle(-100, 80, presher));

	}
}
