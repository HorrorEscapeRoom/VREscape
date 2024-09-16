using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlchemicalCauldron : MonoBehaviour
{        
    public GameObject finishedResult;
    public GameObject finishedResultPerfect;
    public StorageVolume resultLocation;

    public List<AlchemyIngredient> currentItems;

    public List<AlchemyIngredient> solutionItems;

    public bool testing = false;
    public float timer = 5;
    float timer2 = 0;

    bool puzzleSolved = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "XRItem" && other.GetComponent<AlchemyIngredient>() != null)
        {
            //print("Is an alchemical ingredient");
            AddItem(other.transform);            
        }
    }

    void AddItem(Transform itemToAdd)
    {
        AlchemyIngredient item = itemToAdd.GetComponent<AlchemyIngredient>();
        
        if(currentItems.Any(x => x.name == item.name))
        {
            //ALready Exists
            return;
        }
        currentItems.Add(item);
        //Destroy(item.gameObject);
        item.gameObject.SetActive(false);
        CheckSolution();
    }

    /// <summary>
    /// Checks if the three ingredients required for the solution are being held by the alchemy device
    /// </summary>
    public void CheckSolution()
    {
        if (currentItems.ContainsAll(solutionItems))
        {
            PuzzleSolved(true);
        }
        else if(currentItems.Count >= solutionItems.Count)
        {
            ResetPuzzle();
            print("Ingredients Incorrect");
        }
    }


    public void PuzzleSolved(bool perfectSolution)
    {
        if (puzzleSolved != true)
        {
            if(perfectSolution)
            {
                print("Alchemy Cauldron Puzzle Solved Perfectly");
                resultLocation.SetItem(Instantiate(finishedResultPerfect.transform));
            }
            else
            {
                print("Alchemy Cauldron Puzzle Solved Poorly");
                resultLocation.SetItem(Instantiate(finishedResult.transform));
            }
            
            puzzleSolved = true;

            //Clears variables and Destroy objects
            ResetPuzzle();
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

    void ResetPuzzle()
    {
        foreach(var item in currentItems)
        {
            Destroy(item.gameObject);
        }
        currentItems.Clear();
    }
}
