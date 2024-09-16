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
			for (int i = 0; i < liquid_source.Count; i++)
			{
				sum += liquid_source[i].amount;
			}
			float ret = sum + constAmount;			
			return Mathf.Clamp(ret, 0, choke);
		}
	}
	public List<PipePuzzelLiquidHolder> liquid_source = new List<PipePuzzelLiquidHolder>() { };

	public void Start() {

	}

	void Update() {
		Gauge?.UpdateGauge(amount); // for testing remove me
	}

	public PipeGaugeScript Gauge;


}
