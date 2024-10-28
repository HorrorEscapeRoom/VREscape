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
            storage.SetLocks(true, true);
        }
        if(fakeCauldron != null)
        {
            fakeCauldron.SetActive(false);
        }
        if(realCauldron != null)
        {
            BoxCollider box = realCauldron.GetComponent<BoxCollider>();
            box.enabled = false;
            //realCauldron.tag = null;
            BoxCollider[] alchBoxes =  realCauldron.GetComponentsInChildren<BoxCollider>();
            foreach(BoxCollider thisBox in alchBoxes)
            {
                if(thisBox != box)
                {
                    thisBox.enabled = true;
                }
            }
        }
    }

}
