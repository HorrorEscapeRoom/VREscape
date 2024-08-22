using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PipePuzzelSystem;

public class PipePuzzelLiquidHolder : MonoBehaviour
{

	public float constAmount = 0.0f;
	public float choke = 100.0f;

	public void NewChoke(float NewChoke) {
		choke = NewChoke;
	}

	public float amount
	{
		get
		{
			float sum = 0;
			for (int i = 0; i < childen.Count; i++)
			{
				sum += childen[i].amount;
			}
			return sum + constAmount;
		}
	}
	List<PipePuzzelLiquidHolder> childen;
	public List<GameObject> childenObjects;

	public void Start() {
		foreach (GameObject childenObject in childenObjects) {
			childen.Add(childenObject.GetComponent<PipePuzzelLiquidHolder>());
		}
		GaugeScript = Gauge.GetComponent<PipeGaugeScript>();
	}

	 void Update() {
		GaugeScript.UpdateGauge(amount); // for testing remove me
	}

	PipeGaugeScript GaugeScript;
	public GameObject Gauge;


}
