using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGaugeScript : MonoBehaviour
{

	Transform Needle;
	public void UpdateGauge(float presher)
	{
		Needle.localRotation = Quaternion.EulerRotation(Needle.localRotation.y, presher, Needle.localRotation.z);

	}
}
