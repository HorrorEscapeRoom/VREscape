using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlchemicalCauldron : PuzzleBase
{        
    public GameObject finishedResult;
    public GameObject finishedResultPerfect;
    public StorageVolume resultLocation;

    public List<AlchemyIngredient> currentItems;
    public List<AlchemyIngredient> solutionItems;

    bool puzzleSolved = false;
    public bool combinedPuzzle = false;
    public bool bothPuzzlesSolved = false;
    public AlchemyPouring PouringPuzzle;

    private void Start()
    {
        RegisterWithOrchestrator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "XRItem" && other.GetComponent<AlchemyIngredient>() != null && other.GetComponentInParent<PourDetector>() == null)
        {
            if(!puzzleSolved)
            {
                AddItem(other.transform);
            }
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
            PuzzleSolved(false);
        }
    }


    public void PuzzleSolved(bool perfectSolution)
    {
        if (puzzleSolved != true && combinedPuzzle == false)
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
        else if(combinedPuzzle)
        {
            print("Cauldron items completed");
            puzzleSolved = true;
            CheckBothPuzzles(perfectSolution);
        }
    }

    public void CheckBothPuzzles(bool perfectSolution)
    {
        if(!bothPuzzlesSolved)
        {
            if(PouringPuzzle != null)
            {
                if(puzzleSolved && PouringPuzzle.puzzleSolved)
                {
                    bothPuzzlesSolved = true;
                    if (perfectSolution)
                    {
                        print("Alchemy Cauldron Puzzle Solved Perfectly");
                        resultLocation.SetItem(Instantiate(finishedResultPerfect.transform));
                    }
                    else
                    {
                        print("Alchemy Cauldron Puzzle Solved Poorly");
                        resultLocation.SetItem(Instantiate(finishedResult.transform));
                    }
                    OnPuzzleComplete(true);
                }
            }
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
