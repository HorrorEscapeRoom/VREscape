using UnityEngine;

public class CurcuitLightScript : MonoBehaviour
{    
    public GameObject LightPrefab;    
    public bool IsTestLight;

    public Material ActiveColour;
    public Material InactiveColour;

    public void SetColour(bool active)
    {
        if (LightPrefab != null)
        {
            LightPrefab.GetComponent<MeshRenderer>().material = active ? ActiveColour : InactiveColour;
        }        
    }
}
