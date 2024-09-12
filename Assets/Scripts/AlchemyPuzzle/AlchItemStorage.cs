using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchItemStorage : MonoBehaviour
{
    public AlchemyPuzzle puzzle;

    public void OnItemPlaced(Transform item)
    {
        puzzle.OnItemPlaced(item);
    }

    public void OnItemPickedUp(Transform item)
    {
        puzzle.OnItemPickedUp(item);
    }
}
