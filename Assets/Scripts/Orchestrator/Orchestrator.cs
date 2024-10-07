using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Orchestrator : MonoBehaviour
{ 
    public List<ActionEvent> OutputList = new();

    private List<IPuzzle> RegisteredPuzzles = new();

    public void RegisterPuzzle(IPuzzle newPuzzle)
    {
        RegisteredPuzzles.Add(newPuzzle);
    }

    public void UnregisterPuzzle(IPuzzle puzzle)
    {
        RegisteredPuzzles.Remove(puzzle);
    }

    public void OnPuzzleComplete(IPuzzle puzzle)
    {
        var whatever = RegisteredPuzzles.FirstOrDefault(x => x.PuzzleID == puzzle.PuzzleID);
        if (whatever != null)
        {
            ActionEvent resu = OutputList.Single(x => x.Id == whatever.PuzzleID);
            resu.ProcessActions();
        }
    }

    private void ExecuteForType(EnumPuzzleType type)
    {
        var whatever = OutputList.SingleOrDefault(x => x.PuzzleType == type);
        if (whatever != default)
        {
            foreach (UnityEvent Eve in whatever.Actions)
            {
                //Do something
                Debug.Log("Something");
            }
        }
    }
}

[System.Serializable]
public class ActionEvent
{
    public int Id;
    public EnumPuzzleType PuzzleType;
    public List<UnityEvent> Actions = new();

    public void ProcessActions()
    {
        foreach (UnityEvent action in Actions)
        {
            //Do something
            action?.Invoke();
        }
    }
}

