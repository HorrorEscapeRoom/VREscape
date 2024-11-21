using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlchemyPuzzle : MonoBehaviour
{
    public List<StorageVolume> ingredientLocations;

    public List<AlchemyIngredient> currentIngredients;

    public XRItem puzzleSolvedItem;
    public XRItem puzzlePerfectSolvedItem;
    public StorageVolume SolvedLocation;

    public List<AlchemyIngredient> solutionItems;

    public bool testing = true;
    public float timer = 5;
    float timer2 = 0;

    bool puzzleSolved = false;
    public bool finalPuzzle = false;

    /// <summary>
    /// When an item is placed in a storage volume store it as a variable
    /// </summary>
    public void OnItemPlaced(Transform item)
    {        
        if(item.GetComponent <AlchemyIngredient>() != null)
        {
            currentIngredients.Add(item.GetComponent<AlchemyIngredient>());
        }
        CheckSolution();
    }

    public void OnItemPickedUp(Transform item)
    {
        var pickedUpItem = currentIngredients.FirstOrDefault(x => x.name == item.name);
        currentIngredients.Remove(pickedUpItem);
    }

    /// <summary>
    /// Checks if the three ingredients required for the solution are being held by the alchemy device
    /// </summary>
    public void CheckSolution()
    {
        if (currentIngredients.ContainsAll(solutionItems))
        {
            PuzzleSolved(true);
        }
        /*else if(currentIngredients.Count == solutionItems.Count)
        {
            PuzzleSolved(false);
        }*/
    }


    public void PuzzleSolved(bool perfectCompletion)
    {
        if (puzzleSolved != true && !finalPuzzle)
        {
            if (perfectCompletion)
            {
                print("Alchemy Item Puzzle Solved Perfectly");
                SolvedLocation.SetItem(Instantiate(puzzlePerfectSolvedItem.transform));
            }
            else
            {
                print("Alchemy Item Puzzle Solved Poorly");
                SolvedLocation.SetItem(Instantiate(puzzleSolvedItem.transform));
            }
            
            
            puzzleSolved = true;
            
            
            foreach (var item in currentIngredients)
            {
                Destroy(item.gameObject);
            }


        }
        if (finalPuzzle && puzzleSolved != true)
        {
            puzzleSolved = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("CreditsScene");
        }
    }

    private void Update()
    {
        if (testing && timer2 <= Time.time)
        {
            CheckSolution();
            timer2 += Time.time + timer;
        }
    }
}
