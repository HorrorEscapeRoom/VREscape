using UnityEngine;

public class StonePuzzleEchoToParent : MonoBehaviour
{
    
    [SerializeField]
    int Index;
    GemCradle _gemCradle;

    private void Start()
    {
        _gemCradle = gameObject.transform.parent.gameObject.GetComponent<GemCradle>();
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
            _gemCradle.AddGemToSocket(Index, heldItem);
        }
    }

    void OnItemPickedUp(GameObject heldItem)
    {
        _gemCradle.RemoveGemFromSocket(Index);
    }
}
