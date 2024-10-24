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
        //AlchemyIngredient alchemyIng = other.GetComponentInParent<AlchemyIngredient>();
       if(storedIng == "" || alchemyIng.ingredient == storedIng)
        {
            storedIng = alchemyIng.ingredient;
            
            
            PourDetector pouringDetector  = other.GetComponentInParent<PourDetector>();
            if (alchemyIng.fillAmount < receivingDetector.maxFluidAmount)
            {
                if (pouringDetector != null)
                {
                    receivingDetector.liquid.GetComponent<MeshRenderer>().material = pouringDetector.liquid.GetComponent<MeshRenderer>().material;
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

    private void OnTriggerExit(Collider other)
    {
        //pouringDetector = null;
    }
}
