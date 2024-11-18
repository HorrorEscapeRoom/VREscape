using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSink : MonoBehaviour
{
	// Start is called before the first frame update
	public Liquid display_water;
	public PipePuzzelLiquidHolder tap_waterSource;
	public PipePuzzelLiquidHolder drain_waterSource;

	public GameObject KeyPrefab;

	//public Transform Tap_hole_loaction;

	public PourDetector tap;

	private bool key_dispensed = false;

	float fill_amount = 0.0f;
	bool tap_stream_on = false;

	//void Start()
    //{
	//	streamObject = Instantiate(streamPrefab, Tap_hole_loaction.position, Quaternion.identity, transform).GetComponent<Stream>();
	//}
	//[ContextMenu("DoSinkAnimation")]
	//void DoSinkAnimation() {
	//	sinkAnimationEndTime = Time.time + 10.0f;
	//}

    // Update is called once per frame
    void Update()
    {

		display_water.fillAmount = 1 - fill_amount;
		Debug.Log(tap_waterSource.amount);
		if (tap_waterSource.amount > 0.5) {
			fill_amount += tap_waterSource.amount * Time.deltaTime * 0.1f;
			tap.pourThreshold = 0;
			
			//if (!tap_stream_on) streamObject.Begin();
		} else {
			tap.pourThreshold = 255;
			//if (tap_stream_on) streamObject.End();
		}

		fill_amount += drain_waterSource.amount * Time.deltaTime * 0.1f;
		
		fill_amount = Mathf.Clamp(fill_amount,0, 1);

		//Debug.Log($"fill_amount:{fill_amount},tap_waterSource.amount: {tap_waterSource.amount} , drain_waterSource.amount {drain_waterSource.amount} ");

		

		if (drain_waterSource.amount > 0.6 && !key_dispensed) {
			Instantiate(KeyPrefab,gameObject.transform);
			key_dispensed = true;
		}
	}
}
