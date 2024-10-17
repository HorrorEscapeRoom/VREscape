using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightSense : MonoBehaviour
{
    [SerializeField] float angleRange = 30f;
    public UnityEvent<float> OnLightSense;
    bool active = false, hide = false;
    Transform torch;
    void Start()
    {
        torch = FindFirstObjectByType<FlashlightController>().transform;
        OnLightSense?.Invoke(0);
    }
    // Update is called once per frame
    void Update()
    {
        if(active){
            //get the directional difference between the torch forward and this object
            float distance = Vector3.Angle(torch.forward, transform.position - torch.position);
            //Debug.Log($"Distance: {distance}");
            if(distance < angleRange){
                hide = false;
                //Debug.Log("In range");
                OnLightSense.Invoke(ReScale(distance, 0, angleRange, 1, 0));
            }else if(!hide){
                OnLightSense.Invoke(0);
                hide = true;
            }

        }
    }
    float ReScale(float value, float oldMin, float oldMax, float newMin, float newMax){
        return (value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
    }
    public void SetActive(bool value){
        active = value;
        if(!active){
            OnLightSense.Invoke(0);
        }
    }
}
