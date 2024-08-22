using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PipePuzzelSystem : MonoBehaviour
{
	public PipePuzzelLiquidHolder PipePuzzelLiquidHolder;

	float RequiredAmount = 0.0f;

	public UnityEvent<bool> PuzzelDoneFuntion;

    // Update is called once per frame
    void Update()
    {
        if (PipePuzzelLiquidHolder.amount > RequiredAmount) {
			PuzzelDoneFuntion?.Invoke(true);
		} else {
			PuzzelDoneFuntion?.Invoke(false);
		}
    }
}
