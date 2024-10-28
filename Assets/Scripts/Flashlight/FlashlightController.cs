using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    bool flicked = false, on = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Use(float trigger){
        if(trigger > 0.5f && !flicked){
            flicked = true;
            ToggleTorch();
        }else if(trigger < 0.5f){
            flicked = false;
        }
    }
    public void TestToggle(){
        ToggleTorch();
    }

    void ToggleTorch(){
        on = !on;
        transform.GetChild(0).gameObject.SetActive(on);
        Debug.Log($"Torch: {on}");
        LightSense[] senses = FindObjectsByType<LightSense>(FindObjectsSortMode.None);
        for(int i = 0; i < senses.Length; i++){
            senses[i].SetActive(on);
        }
    }
}
