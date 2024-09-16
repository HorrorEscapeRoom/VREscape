using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSink : MonoBehaviour
{
	// Start is called before the first frame update
	public Liquid display_water;
	public PipePuzzelLiquidHolder waterSource;

	void Start()
    {
        
    }
	//[ContextMenu("DoSinkAnimation")]
	//void DoSinkAnimation() {
	//	sinkAnimationEndTime = Time.time + 10.0f;
	//}

    // Update is called once per frame
    void Update()
    {
		display_water.fillAmount = waterSource.amount;
	}
}
