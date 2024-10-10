using UnityEngine;

public class PuzzleBase : MonoBehaviour, IPuzzle
{
    private PuzzleOrchestrator _orchestrator;

    [field: SerializeField]
    public int PuzzleId { get; set; }
    [field: SerializeField]
    public EnumPuzzleType PuzzleType { get; set; }

    private void Awake()
    {
        _orchestrator = gameObject.GetComponentInParent<PuzzleOrchestrator>();
        if (_orchestrator == null)
        {
            Debug.LogError($"PuzzleBase->{this.name} - Unable to find PuzzleOrchestrator reference in parent. Ensure there is an instance of PuzzleOrchestrator" +
                           $" in a parent GameObject relative to this class.");
        }
    }

    /// <summary>
    /// Registers with the PuzzleOrchestrator
    /// </summary>
    public virtual void RegisterWithOrchestrator()
    {
        if (_orchestrator == null)
        {
            Debug.LogError($"PuzzleBase->{this.name} - Error in RegisterWithOrchestrator(): PuzzleOrchestrator is null.");
            return;
        }
        _orchestrator.RegisterPuzzle(this);
    }
    
    /// <summary>
    /// Unregisters with PuzzleOrchestrator
    /// </summary>
    public virtual void UnRegisterWithOrchestrator()
    {
        _orchestrator.UnregisterPuzzle(this);
    }

    /// <summary>
    /// Actions to be taken when the puzzle is activated.
    /// This will either be at initialization, or when activated from an external source
    /// </summary>
    public virtual void OnPuzzleAwake()
    {

    }

    /// <summary>
    /// Notifies the PuzzleOrchestrator when the puzzle has completed.
    /// </summary>
    public virtual void OnPuzzleComplete()
    {
        if (_orchestrator == null)
        {
            Debug.LogError($"PuzzleBase->{this.name} - Error in OnPuzzleComplete(): PuzzleOrchestrator is null.");
            return;
        }
        _orchestrator.OnPuzzleComplete(this);
    }
}
// USAGE:
// Inherit PuzzleBase
// Example: public class MyPuzzleNameHere: PuzzleBase
// PuzzleBase implements MonoBehaviour so you can replace the default MonoBehaviour inheritance with Puzzlebase.
//
// Add RegisterPuzzle() to Start()
//
// Add UnregisterPuzzle() if you need to remove the puzzle from the PuzzleOrchestrator. 
//     This will disable the orchestrator from processing OnPuzzleComplete events.
//
// Override OnPuzzleAwake() if you puzzle requires activation from an external source.
//
// OnPuzzleComplete() will notify the PuzzleOrchestrator that the puzzle has been completed.
