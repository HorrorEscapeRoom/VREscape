using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



public class AlchemyPouring : MonoBehaviour
{
    public GameObject finishedResult;
    public StorageVolume resultLocation;
    public AlchemyIngredient solutionResult;
    public Material solutionMaterial;
    public float solutionAmount;
    public bool resultIsFluid = false;

    public int maxInputItemCount = 3;

    public float maxFill = 200f;
    public bool useFill = false;
    public GameObject fillObj;

    public List<AlchemyIngredient> InputItems;
    public List<float> itemAmounts;
    public List<AlchemyIngredient> SolutionItems;

    bool puzzleSolved = false;

    private void OnTriggerEnter(Collider other)
    {               
        if (other.GetComponentInParent<AlchemyIngredient>() != null)
        {
            // If we are already full
            if (InputItems.Count >= maxInputItemCount)
            {
                // This should not be possible.
                Debug.Log("InputItems already at max");
                return;
            }

            var newItem = other.GetComponentInParent<AlchemyIngredient>();
           
            if (InputItems.Any(x => x.name == newItem.name))
            {
                // Already exists
                Debug.Log($"{newItem.name} already exists in InputItems");
                return;
            }
                        
            InputItems.Add(newItem);
            float newAmount = 0;
            itemAmounts.Add(newAmount);
            Debug.Log($"{newItem} added to InputItems");

            VisualFill();
            CheckSolution();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        var newItem = other.GetComponentInParent<AlchemyIngredient>();

        var whatever = InputItems.FirstOrDefault(x => x.name == newItem.name);
        var index = InputItems.IndexOf(newItem);

        if (whatever == null)         
        {
            return;
        }
        
        /*if (whatever.fillAmount < maxFill)
        {
            whatever.fillAmount += 1f;
        }*/
        if (itemAmounts[index] < maxFill)
        {
            itemAmounts[index] += 1f;
        }

        VisualFill();
        CheckSolution();
    }

    private void OnTriggerExit(Collider other)
    {
        CheckSolution();
    }

    void CheckSolution()
    {
        if (InputItems.ContainsAll(SolutionItems))
        {
            // All SolutionItems exist in InputItems
            if (useFill)
            {
                /*if (InputItems.All(val => val.fillAmount >= maxFill))
                {
                    PuzzleSolved();
                }*/
                if(itemAmounts.All(val => val >= maxFill))
                {
                    PuzzleSolved();
                }
            }
            else
            {
                PuzzleSolved();
            }                     
        }
        else
        {
            //Failed
            if (useFill && InputItems.Count == maxInputItemCount)
            {
                if(itemAmounts.All(val => val >= maxFill))
                {
                    ResetItems();
                    print("Ingredients Incorrect - Puzzle Failed");
                    VisualFill();
                    //TODO: Smoke/Fizzle effect
                }
            }       
        }
    }

    public void PuzzleSolved()
    {
        if (!puzzleSolved)
        {            
            Debug.Log("Alchemy Puzzle Solved");
            if (resultIsFluid)
            {
                solutionAmount = maxFill * InputItems.Count;
                //Set this puzzles Alch ingr to the solution
                gameObject.GetComponent<AlchemyIngredient>().ingredient = solutionResult.ingredient;
                gameObject.GetComponent<AlchemyIngredient>().fillAmount = solutionAmount;
                gameObject.GetComponent<PourDetector>().SetFluid(solutionResult);
                if(solutionMaterial != null) //Set fluid Shader
                {
                    fillObj.GetComponent<MeshRenderer>().material = solutionMaterial;
                }
            }
            else
            {
                resultLocation.SetItem(Instantiate(finishedResult.transform));
            }

            puzzleSolved = true;
            ResetItems();
        }
    }

    private void ResetItems()
    {
        //Clears variables and Destroy objects
        /*foreach(var item in InputItems)
        {
            item.fillAmount = 0;
        }*/
        InputItems.Clear();
        itemAmounts.Clear();
    }

    void VisualFill()
    {
        if (fillObj == null)
            return;

        //Using which ever object
        if (!puzzleSolved)
        {
            float totalFillAmount = itemAmounts.Sum(x => x);//InputItems.Sum(x => x.fillAmount);

            fillObj.transform.localScale = new Vector3(0.8f, (totalFillAmount / (maxFill * SolutionItems.Count))-0.1f, 0.8f);

            //fillObj.transform.localPosition = new Vector3(fillObj.transform.localPosition.x, (totalFillAmount / (maxFill * SolutionItems.Count)) - 1f, fillObj.transform.localPosition.z);
        }
        else if (puzzleSolved)
        {            
            fillObj.transform.localScale = new Vector3(0.8f, (solutionAmount / (maxFill * SolutionItems.Count))-0.1f, 0.8f);
        }
        //Using liquid shader
        /*if (liquid != null)// 0.3 is Full 0.7 is empty
        {
            liquid.fillAmount = Mathf.Lerp(0.65f, 0.4f, (fluid.fillAmount / maxFluidAmount));
        }*/
    }
}
