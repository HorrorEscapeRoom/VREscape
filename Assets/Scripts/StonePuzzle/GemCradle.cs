using System.Collections;
using System.Collections.Generic;
using StonePuzzle;
using UnityEngine;


public class GemCradle : MonoBehaviour
{
    public List<GemstoneSocket> gemSockets;

    
    
    public void AddGemToSocket(int socketIndex, GameObject gemObject)
    {			
        if (gemSockets.Count == 0 || socketIndex > gemSockets.Count - 1)
        {
            Debug.LogError($"GemCradle - Invalid socketIndex in AddGemToSocket: ({socketIndex})");
            return;
        }

        gemSockets[socketIndex].gemObject = gemObject;                             
        UpdateAllSocketState();
    }

    public void RemoveGemFromSocket(int socketIndex) 
    {
        if (gemSockets.Count == 0 || socketIndex > gemSockets.Count - 1)
        {
            Debug.LogError($"GemCradle - Invalid socketIndex in RemoveGemToSocket: ({socketIndex})");
            return;
        }

        gemSockets[socketIndex].gemObject = null;                   
        UpdateAllSocketState();
    }   
    
    public void UpdateAllSocketState()
    {
        // Do something
    }
}
