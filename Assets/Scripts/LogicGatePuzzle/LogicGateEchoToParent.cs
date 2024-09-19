using Assets.Scripts.LogicGatePuzzle;
using UnityEngine;

public class LogicGateEchoToParent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    int Index;
    CircuitBoard CircuitBoard;

    private void Start()
    {
        CircuitBoard = gameObject.transform.parent.gameObject.GetComponent<CircuitBoard>();
    }

    void OnItemPlaced(GameObject heldItem)
    {
        if (heldItem == null)
        {
            return;
        }

        var logicItem = heldItem.GetComponent<LogicGateScript>();
        if (logicItem)
        {
            CircuitBoard.AddICToSocket(Index, heldItem);
        }
    }

    void OnItemPickedUp(GameObject heldItem)
    {
        CircuitBoard.RemoveICFromSocket(Index);
    }
}
