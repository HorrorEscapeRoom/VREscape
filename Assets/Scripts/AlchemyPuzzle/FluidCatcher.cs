using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCatcher : MonoBehaviour
{
    public PourDetector receivingDetector;
    public AlchemyIngredient alchemyIng;
    public PourDetector pouringDetector;
    public float pourAmount = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<AlchemyIngredient>() != null)
        {
            //print(other.GetComponentInParent<AlchemyIngredient>().ingredient + " is being poured");
            if (alchemyIng.ingredient == "" && other.GetComponentInParent<PourDetector>() != null)//receivingDetector.fluid.ingredient == "")
            {
                alchemyIng.ingredient = other.GetComponentInParent<AlchemyIngredient>().ingredient;
                pouringDetector = other.GetComponentInParent<PourDetector>();
                receivingDetector.liquid.GetComponent<MeshRenderer>().material = pouringDetector.liquid.GetComponent<MeshRenderer>().material;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<AlchemyIngredient>() != null) //&& this != other.GetComponentInParent<FluidCatcher>())
        {
            if (other.GetComponentInParent<AlchemyIngredient>().ingredient == alchemyIng.ingredient && alchemyIng.fillAmount < receivingDetector.maxFluidAmount)
            {
                if (pouringDetector != null)
                {
                    if (pouringDetector.onlyEmptyWhenFilling)
                    {
                        pouringDetector.fluid.fillAmount -= pourAmount;
                        pouringDetector.UpdateVisual();
                    }
                    alchemyIng.fillAmount += pourAmount;
                    receivingDetector.UpdateVisual();
                }
                else
                {
                    other.GetComponentInParent<PourDetector>();
                }
                /*if (pouringDetector.onlyEmptyWhenFilling)
                {
                    pouringDetector.fluid.fillAmount -= 1f;
                    pouringDetector.UpdateVisual();
                }*/
                /*alchemyIng.fillAmount += 1f;
                receivingDetector.UpdateVisual();*/
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //pouringDetector = null;
    }
}
