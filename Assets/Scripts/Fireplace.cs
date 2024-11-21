using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour
{
    public GameObject fire;
    public GameObject ashes;

    public void PutOut()
    {
        ashes.SetActive(true);
        Destroy(fire);
    }
}
