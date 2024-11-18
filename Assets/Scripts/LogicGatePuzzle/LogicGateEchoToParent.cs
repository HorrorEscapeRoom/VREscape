using Assets.Scripts.LogicGatePuzzle;
using Unity.VisualScripting;
using UnityEngine;

public class LogicGateEchoToParent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    int Index;
    CircuitBoard CircuitBoard;

    private void Start()
    {
        CircuitBoard = gameObject.transform.parent.parent.gameObject.GetComponent<CircuitBoard>();
    }

    public void OnItemPlaced(Transform heldItem)
    {
        if (heldItem == null)
        {
            return;
        }

        var logicItem = heldItem.GetComponent<LogicGateScript>();
        if (logicItem)
        {
            CircuitBoard.AddICToSocket(Index, heldItem.gameObject);
        }
    }

    public void OnItemPickedUp(Transform heldItem)
    {
        CircuitBoard.RemoveICFromSocket(Index);
    }
}
