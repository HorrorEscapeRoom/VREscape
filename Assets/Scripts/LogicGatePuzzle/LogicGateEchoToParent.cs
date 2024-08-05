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
        var whatever = heldItem.GetComponent<LogicGateScript>();
        var gatetype = whatever.gateType; 

        CircuitBoard.UpdateLogic(Index, gatetype);
    }

    void OnItemPickedUp(GameObject heldItem)
    {
        CircuitBoard.UpdateLogic(Index, EnumLogicGateType.UNSET);
    }
}
