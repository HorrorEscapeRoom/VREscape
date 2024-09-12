using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSink : MonoBehaviour
{
	// Start is called before the first frame update
	public Transform SinkWater;

	private float sinkAnimationEndTime = 0.0f;

	static Vector3 waterStart = new Vector3(0,0,0);
	static Vector3 waterEnd = new Vector3(0, 20, 0);

	void Start()
    {
        
    }
	[ContextMenu("DoSinkAnimation")]
	void DoSinkAnimation() {
		sinkAnimationEndTime = Time.time + 10.0f;
	}

    // Update is called once per frame
    void Update()
    {
		SinkWater.localPosition = Vector3.Lerp(waterStart, waterEnd, (Time.time - sinkAnimationEndTime )/ 100.0f);
		Debug.Log(SinkWater.localPosition);
	}
}
