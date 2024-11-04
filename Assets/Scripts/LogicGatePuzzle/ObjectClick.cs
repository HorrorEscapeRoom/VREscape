using Assets.Scripts.LogicGatePuzzle;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class ObjectClick : MonoBehaviour
{
    public GameObject CircuitBoard;

    // Update is called once per frame
    void Update()
    {        
        if (Mouse.current.leftButton.wasReleasedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ////if (hit.transform.name.Contains("Switch"))
                ////{
                ////    Debug.Log($"Hit switch {hit.transform.name} on object {hit.transform.parent.name}");
                ////}

                if (hit.transform.name == "Switch0" || hit.transform.name == "Switch1" || hit.transform.name == "Switch2")
                {

                }

                var whatever = CircuitBoard.GetComponent<CircuitBoard>() as CircuitBoard;

                ////if (whatever.transform.name != CircuitBoard.name)
                ////{
                ////    Debug.Log($"Circuitboard names did not match.");
                ////    return;
                ////}

                try
                {
                    switch (hit.transform.name)
                    {
                        case "Switch0":
                            whatever.SwitchList[0].active = !whatever.SwitchList[0].active;
                            Debug.Log($"Switch0 set to {whatever.SwitchList[0].active}");
                            if (whatever.SwitchList[0].active)
                            {
                                whatever.SwitchList[0].SwitchObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.green;
                            }
                            else
                            {
                                whatever.SwitchList[0].SwitchObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.red;
                            }
                            break;
                        case "Switch1":
                            whatever.SwitchList[1].active = !whatever.SwitchList[1].active;
                            Debug.Log($"Switch1 set to {whatever.SwitchList[1].active}");
                            if (whatever.SwitchList[1].active)
                            {
                                whatever.SwitchList[1].SwitchObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.green;
                            }
                            else
                            {
                                whatever.SwitchList[1].SwitchObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.red;
                            }
                            break;
                        case "Switch2":
                            whatever.SwitchList[2].active = !whatever.SwitchList[2].active;
                            Debug.Log($"Switch2 set to {whatever.SwitchList[2].active}");
                            if (whatever.SwitchList[2].active)
                            {
                                whatever.SwitchList[2].SwitchObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.green;
                            }
                            else
                            {
                                whatever.SwitchList[2].SwitchObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.red;
                            }
                            break;
                    }

                    whatever.UpdateAllSocketState();
                }
                catch (Exception ex)
                {
                    
                }                
            }
        }
    }

    public void OnButtonPressed(int index)
    {
        var whatever = CircuitBoard.GetComponent<CircuitBoard>() as CircuitBoard;
        whatever.SwitchList[index].active = !whatever.SwitchList[index].active;
        Debug.Log($"Switch0 set to {whatever.SwitchList[0].active}");
        if (whatever.SwitchList[index].active)
        {
            whatever.SwitchList[index].SwitchObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.green;
        }
        else
        {
            whatever.SwitchList[index].SwitchObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.red;
        }
        whatever.UpdateAllSocketState();
    }
}
