using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSink : MonoBehaviour
{
	// Start is called before the first frame update
	public Liquid water;
	float sinkAnimationEndTime;

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
		water.fillAmount = Mathf.Lerp(0, 1, (Time.time - sinkAnimationEndTime )/ 100.0f);
	}
}
