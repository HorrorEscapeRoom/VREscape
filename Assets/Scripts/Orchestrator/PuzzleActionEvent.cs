using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class PuzzleActionEvent
{
    public int id;
    public EnumPuzzleType puzzleType;
    public bool successful;
    public List<UnityEvent> actions = new();

    public void ProcessActions()
    {
        foreach (var action in actions)
        {
            action?.Invoke();
        }
    }
}