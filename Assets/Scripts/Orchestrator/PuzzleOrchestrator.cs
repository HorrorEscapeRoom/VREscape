using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleOrchestrator : MonoBehaviour
{ 
    public List<PuzzleActionEvent> outputList = new();
    private List<IPuzzle> _registeredPuzzles = new();
    private bool _verboseLogs;

    private void Awake()
    {
        _verboseLogs = true;
        if (_verboseLogs)
        {
            Debug.Log($"PuzzleOrchestrator has awoken on GameObject({gameObject.name})");
        }
    }

    public void RegisterPuzzle(IPuzzle puzzle)
    {
        if (_verboseLogs)
        {
            Debug.Log($"PuzzleOrchestrator->RegisterPuzzle: Puzzle registered with id-> '{puzzle.PuzzleId}'");
        }
        _registeredPuzzles.Add(puzzle);
    }

    public void UnregisterPuzzle(IPuzzle puzzle)
    {
        if (_verboseLogs)
        {
            Debug.Log($"PuzzleOrchestrator->UnregisterPuzzle: Puzzle unregistered with id-> '{puzzle.PuzzleId}'");
        }
        _registeredPuzzles.Remove(puzzle);
    }

    public void OnPuzzleComplete(IPuzzle puzzle, bool success)
    {
        // Gets the registered puzzle where matching puzzle ID's
        var puzzleResult = _registeredPuzzles.SingleOrDefault(x => x.PuzzleId == puzzle.PuzzleId);
        if (puzzleResult == null)
        {
            if (_verboseLogs)
            {
                Debug.LogWarning($"PuzzleOrchestrator->OnPuzzleComplete - No registered puzzle found for id: '{puzzle.PuzzleId}'");
            }
            return;
        }

        // Checks to see if there are any output events matching the Puzzle ID and success flag.
        // This allows 2 different output states to be defined:
        // Success = True -> The puzzle completed in the success state
        // Success = False -> The puzzle was not successful. This might be that the puzzle has been reset or no longer "Complete"
        var outputResult = outputList.SingleOrDefault(x => x.id == puzzleResult.PuzzleId & x.successful == success);
        if (outputResult == null)
        {
            if (_verboseLogs)
            {
                Debug.LogWarning($"PuzzleOrchestrator->OnPuzzleComplete - Registered puzzle found for PuzzleID/Success: {puzzle.PuzzleId}, {success} but it has no output events.");
            }
            return;
        }

        if (_verboseLogs)
        {
            Debug.Log($"PuzzleOrchestrator->OnPuzzleComplete - Found {outputResult.actions.Count} actions for PuzzleID/Success: {puzzle.PuzzleId}, {success}");
        }

        outputResult.ProcessActions();
    }
}



