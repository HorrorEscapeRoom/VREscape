using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRSlider : MonoBehaviour
{
    public UnityEvent<float> OnValueChange;
    public float value = 0;
    bool active = false;
    float lineLength;
    Vector3 lineDirection;
    Transform slider, hand;
    Vector3 sliderStart, sliderEnd;
    VRHudManager hud;
    // Start is called before the first frame update
    void Start()
    { 
        hud = FindFirstObjectByType<VRHudManager>();
        slider = transform.GetChild(1); 
        sliderStart = transform.GetChild(2).position;
        sliderEnd = transform.GetChild(3).position;
        lineDirection = sliderEnd - sliderStart;
        lineLength = lineDirection.magnitude;
        lineDirection.Normalize();
        hud.DrawLine(sliderStart, sliderEnd, 10000, Color.green);
        
    }
    void Update(){
        if(active){ 
            slider.position = ClosestPointOnLine(hand.position); //FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU FUCK YOU
            //slider.position = ClosestPointOnLine(hand.position, sliderStart, sliderEnd);
            hud.DrawLine(slider.position, hand.position, 0.02f, Color.red);
            if(Vector3.Distance(slider.position, hand.position) > 0.2f){
                hand = null;
                active = false;
            }
        }
    }
    Vector3 ClosestPointOnLine(Vector3 inputPosition)
    {
        Vector3 vectorToInput = inputPosition - sliderStart;
        float projectionLength = Vector3.Dot(vectorToInput, lineDirection);
        projectionLength = Mathf.Clamp(projectionLength, 0, lineLength);
        value = projectionLength / lineLength;
        //hud.Debug($"slider: {value}");
        OnValueChange?.Invoke(value);
        return sliderStart + lineDirection * projectionLength;
    }
    Vector3 ClosestPointOnLine(Vector3 inputPosition, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineDirection = lineEnd - lineStart;
        float lineLength = lineDirection.magnitude;
        lineDirection.Normalize();

        Vector3 vectorToInput = inputPosition - lineStart;
        float projectionLength = Vector3.Dot(vectorToInput, lineDirection);
        projectionLength = Mathf.Clamp(projectionLength, 0, lineLength);

        value = projectionLength / lineLength;
        OnValueChange?.Invoke(value);
        return lineStart + lineDirection * projectionLength;
    }
    void OnGrab(Transform hand)
    { this.hand = hand; active = true; }
    void OnRelease()
    { hand = null; active = false; }
    
}
