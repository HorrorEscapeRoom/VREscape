using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class AlchemyPouring : MonoBehaviour
{
    public GameObject solutionResultItem;
    public StorageVolume resultLocation;
    public AlchemyIngredient solutionResult;
    public AlchemyIngredient solutionResultPerfect;
    public Material solutionMaterial;
    public Material solutionMaterialPerfect;
    public float solutionAmount;
    public bool resultIsFluid = false;

    public int maxInputItemCount = 3;

    public float maxFill = 200f;
    public bool useFill = false;
    public Liquid liquid;

    public List<AlchemyIngredient> InputItems;
    public List<float> itemAmounts;
    public List<AlchemyIngredient> SolutionItems;

    public bool puzzleSolved = false;

    public AlchemicalCauldron cauldron;

    private void OnTriggerEnter(Collider other)
    {               
        if (other.GetComponentInParent<AlchemyIngredient>() != null && other.GetComponentInParent<PourDetector>())
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
        if(other.GetComponentInParent<AlchemyIngredient>() != null && other.GetComponentInParent<PourDetector>())
        {

            var newItem = other.GetComponentInParent<AlchemyIngredient>();

            var whatever = InputItems.FirstOrDefault(x => x.name == newItem.name);
            var index = InputItems.IndexOf(newItem);

            if (whatever == null)         
            {
                return;
            }

            if (itemAmounts[index] < maxFill)
            {
                itemAmounts[index] += 1f;
            }

            VisualFill();
            CheckSolution();
        }
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
                if(itemAmounts.All(val => val >= maxFill))
                {
                    PuzzleSolved(true);
                    Debug.Log("Alchemy Puzzle Solved - Pouring Perfect Ingredients");
                }
            }
            else
            {
                PuzzleSolved(true);
            }                     
        }
        else
        {
            //Failed
            if (useFill && InputItems.Count == maxInputItemCount)
            {
                if(itemAmounts.All(val => val >= maxFill))
                {
                    Debug.Log("Alchemy Puzzle Solved - Pouring Incorrect Ingredients");
                    PuzzleSolved(false);
                }
            }       
        }
    }

    public void PuzzleSolved(bool perfectCompletion)
    {
        if (!puzzleSolved)
        {
            if (resultIsFluid)
            {
                solutionAmount = maxFill * InputItems.Count;
                //Set this puzzles Alch ingr to the solution
                if(perfectCompletion)
                {
                    gameObject.GetComponent<AlchemyIngredient>().ingredient = solutionResultPerfect.ingredient;
                    if (solutionMaterialPerfect != null) //Set fluid Shader
                    {
                        liquid.GetComponent<MeshRenderer>().material = solutionMaterialPerfect;
                    }
                }
                else
                {
                    gameObject.GetComponent<AlchemyIngredient>().ingredient = solutionResult.ingredient;
                    if (solutionMaterial != null) //Set fluid Shader
                    {
                        liquid.GetComponent<MeshRenderer>().material = solutionMaterial;
                    }
                }
                
                gameObject.GetComponent<AlchemyIngredient>().fillAmount = solutionAmount;
                gameObject.GetComponent<PourDetector>().SetFluid(solutionResult);
            }
            else
            {
                if(solutionResultItem != null)
                {
                    resultLocation.SetItem(Instantiate(solutionResultItem.transform));
                }
            }

            puzzleSolved = true;
            ResetItems();
            if(cauldron != null)
            {
                cauldron.CheckBothPuzzles(perfectCompletion);
            }
        }
    }

    private void ResetItems()
    {
        InputItems.Clear();
        itemAmounts.Clear();
    }

    void VisualFill()
    {
        if (liquid == null)
            return;
        float totalFillAmount = itemAmounts.Sum(x => x);

        if(!puzzleSolved)
        {
            liquid.fillAmount = Mathf.Lerp(0.7f, 0.3f, (totalFillAmount/ (maxFill*maxInputItemCount)));
        }
    }
}
