using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 45;
    public Transform origin;
    public GameObject streamPrefab;

    public bool yAxisPour = false;
    bool isPouring = false;
    Stream currentStream;

    public bool infiniteFluid = true;
    public bool onlyEmptyWhenFilling = false;
    public float maxFluidAmount = 200f;
    public AlchemyIngredient fluid;

    public Liquid liquid;

    private void Start()
    {
        if(!infiniteFluid)
        {
            UpdateVisual();
        }        
    }

    private void Update()
    {
        bool pourCheck = CalculatePouringAngle() < pourThreshold;
        if (isPouring != pourCheck )
        {
            isPouring = pourCheck;            
            if(isPouring && (infiniteFluid || (fluid.fillAmount > 0 && fluid != null)))//(currentFluidAmount > 0 && fluid != null)))
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
        UpdateFluidAmount();
    }
    
    private void StartPour()
    {
        currentStream = CreateStream();
        currentStream.Begin();
    }

    void EndPour()
    {
        if(currentStream != null )
        {
            currentStream.End();
            currentStream = null;
        }
    }

    float CalculatePouringAngle()
    {
        if(yAxisPour)
        {
            return transform.forward.y * Mathf.Rad2Deg;
        }
        else
        {
            return transform.forward.z * Mathf.Rad2Deg;
        }
    }

    Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }

    void UpdateFluidAmount()
    {
        if (infiniteFluid)
        {
            return;
        }
        if (isPouring && fluid.fillAmount > 0 && fluid != null && !onlyEmptyWhenFilling)//currentFluidAmount > 0)
        {
            fluid.fillAmount -= 1f;
            UpdateVisual();
        }
        if (fluid.fillAmount <= 0 && fluid != null && currentStream != null) //currentFluidAmount <= 0 &&
        {
            isPouring = false;
            gameObject.GetComponent<AlchemyIngredient>().ingredient = null;
            UpdateVisual();
            //fluid = null;
        }
    }

    public void SetFluid(AlchemyIngredient ingredient)
    {
        fluid.ingredient = ingredient.ingredient;
        //fluid = ingredient;
    }

    public void UpdateVisual()
    {
        if (liquid != null)// 0.3 is Full 0.7 is empty
        {
            liquid.fillAmount = Mathf.Lerp(0.65f, 0.4f, (fluid.fillAmount / maxFluidAmount));
        }
    }
}