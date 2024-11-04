using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronPlacement : MonoBehaviour
{
    public GameObject fakeCauldron;
    public StorageVolume storage;
    public bool cauldronPlaced = false;
    public GameObject realCauldron;
    // Start is called before the first frame update
    void Start()
    {
        if(fakeCauldron != null)
        {
            fakeCauldron.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnItemPlaced(Transform transform)
    {        
        realCauldron = transform.gameObject;
        cauldronPlaced = true;

        if (storage != null)
        {
            storage.SetLocks(false, false);
            //storage.GetComponent<Collider>().enabled = false;
        }
        if(fakeCauldron != null)
        {
            fakeCauldron.SetActive(false);
        }
        if(realCauldron != null)
        {
            //realCauldron.GetComponent<Collider> ().enabled = true;
            /*BoxCollider box = realCauldron.GetComponent<BoxCollider>();
            box.enabled = false;
            Rigidbody rb =realCauldron.gameObject.GetComponent<Rigidbody>();*/
            

            //realCauldron.tag = null;
            /*BoxCollider[] alchBoxes = realCauldron.GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider thisBox in alchBoxes)
            {
                //thisBox.enabled = true;
                if (thisBox != box)
                {
                    thisBox.enabled = true;
                }
            }*/
            /*realCauldron.GetComponent<Rigidbody>().isKinematic = true;
            realCauldron.GetComponent<Rigidbody>().*/

        }
    }

}
