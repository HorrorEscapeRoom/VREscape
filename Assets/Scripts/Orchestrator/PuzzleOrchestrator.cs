using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleOrchestrator : MonoBehaviour
{ 
    public List<PuzzleActionEvent> outputList = new();

    private List<IPuzzle> registeredPuzzles = new();

    public void RegisterPuzzle(IPuzzle newPuzzle)
    {
        registeredPuzzles.Add(newPuzzle);
    }

    public void UnregisterPuzzle(IPuzzle puzzle)
    {
        registeredPuzzles.Remove(puzzle);
    }

    public void OnPuzzleComplete(IPuzzle puzzle)
    {
        var puzzleResult = registeredPuzzles.SingleOrDefault(x => x.PuzzleId == puzzle.PuzzleId);
        if (puzzleResult == null) 
            return;
        
        var outputResult = outputList.SingleOrDefault(x => x.id == puzzleResult.PuzzleId);
        if (outputResult == null)
            return;
        
        outputResult.ProcessActions();
    }
}



