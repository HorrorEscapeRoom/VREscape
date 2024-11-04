using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    [Tooltip("Angle at which pouring activates")]
    public int pourThreshold = 45;
    [Tooltip("Location where the stream is created")]
    public Transform origin;
    public GameObject streamPrefab;

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
        bool pourCheck = CanPour();
        if (pourCheck)
        {
            CreateRaycast();
        }
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
    
    public bool CanPour()
    {
        return Vector3.Angle(transform.up, Vector3.up) > pourThreshold;
    }

    private void StartPour()
    {
        currentStream = CreateStream();
        currentStream.Begin();

    }

    void CreateRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(origin.position,Vector3.down, out hit);

        hit.transform.gameObject.TryGetComponent<FluidCatcher>(out FluidCatcher fc);
        fc?.Pour(gameObject);
        AlchemyPouring pourPuzzle;
        /*hit.transform.gameObject.TryGetComponent<AlchemyPouring>(out AlchemyPouring pourPuzzle);
        pourPuzzle?.FirstPour(gameObject);
        pourPuzzle?.Pour(gameObject);*/
        try
        {
            pourPuzzle = hit.transform.gameObject.GetComponentInChildren<AlchemyPouring>();
            pourPuzzle?.FirstPour(gameObject);
            pourPuzzle?.Pour(gameObject);
            pourPuzzle = hit.transform.gameObject.GetComponent<AlchemyPouring>();
            pourPuzzle?.FirstPour(gameObject);
            pourPuzzle?.Pour(gameObject);
            pourPuzzle = hit.transform.gameObject.GetComponentInParent<AlchemyPouring>();
            pourPuzzle?.FirstPour(gameObject);
            pourPuzzle?.Pour(gameObject);
        }
        catch (Exception ex)
        {

        }
    }

    void EndPour()
    {
        if(currentStream != null )
        {
            currentStream.End();
            currentStream = null;
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
            gameObject.GetComponent<AlchemyIngredient>().ingredient = "";
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