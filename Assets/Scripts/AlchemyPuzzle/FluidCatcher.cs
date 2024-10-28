using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCatcher : MonoBehaviour
{
    [SerializeField]
    PourDetector receivingDetector;
    public AlchemyIngredient alchemyIng;
    //public PourDetector pouringDetector;*/
    public float pourAmount = 1f;
    public string storedIng = "";

    public void Pour(GameObject other)
    {
        if (receivingDetector.CanPour())
        {
            return;
        }
        if (alchemyIng.ingredient == "")
        {
            storedIng = other.GetComponent<AlchemyIngredient>().ingredient;
            alchemyIng.ingredient = storedIng;
            PourDetector pouringDetector = other.GetComponentInParent<PourDetector>();
            receivingDetector.liquid.GetComponent<MeshRenderer>().material = pouringDetector.liquid.GetComponent<MeshRenderer>().material;
        }

        if (alchemyIng.ingredient == other.GetComponent<AlchemyIngredient>().ingredient)
        {
            PourDetector pouringDetector  = other.GetComponentInParent<PourDetector>();
            if (alchemyIng.fillAmount < receivingDetector.maxFluidAmount)
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
            }
            
        }
       
    }
}
