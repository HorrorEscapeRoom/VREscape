using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour
{
    public GameObject fire;
    public GameObject ashes;
    bool ashesCreated = false;

    public void PutOut()
    {
        if (!ashesCreated)
        {
            ashes.SetActive(true);
            //Instantiate(ashes, ashes.transform);
            print("Ashes Created");
            ashesCreated = true;
        }        
        if(fire != null)
        {
            Destroy(fire);
        }
    }
}
