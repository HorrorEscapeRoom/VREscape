using System.Collections;
using UnityEngine;

public class ItemRestock : MonoBehaviour
{
    [SerializeField] StorageVolume[] restock;
    [SerializeField] float restockTime = 3f;
    [SerializeField] GameObject restockWith;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Restock());
    }
    IEnumerator Restock()
    {
        while (true)
        {
            foreach (StorageVolume storage in restock)
            {
                if(storage.CanPlace()){
                    Transform item = Instantiate(restockWith, storage.transform.position, storage.transform.rotation).transform;
                    storage.SetItem(item);
                }
            }
            yield return new WaitForSeconds(restockTime);
        }
    }
}
