using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PipePuzzelSystem;

public class PipePuzzelLiquidHolder : MonoBehaviour
{

	public float constAmount = 0.0f;
	public float choke = 1.0f;

	public void NewChoke(float NewChoke) {
		choke = NewChoke/360;
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
			float ret = sum + constAmount;			
			return Mathf.Clamp(ret, 0, choke);
		}
	}
	List<PipePuzzelLiquidHolder> childen = new List<PipePuzzelLiquidHolder>() { };
	public List<GameObject> childenObjects;

	public void Start() {
		foreach (GameObject childenObject in childenObjects) {
			childen.Add(childenObject.GetComponent<PipePuzzelLiquidHolder>());
		}
	}

	 void Update() {
		Gauge?.UpdateGauge(amount); // for testing remove me
	}

	public PipeGaugeScript Gauge;


}
