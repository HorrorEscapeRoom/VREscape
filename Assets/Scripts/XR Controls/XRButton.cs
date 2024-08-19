using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRButton : MonoBehaviour
{
    public UnityEvent OnButtonPressed;
    int activeTicks = 0;
    void OnTriggerStay(Collider col){
        if(activeTicks == 0){
            activeTicks = 3;
            OnButtonPressed?.Invoke();
            FindFirstObjectByType<VRHudManager>().Debug("Button Pressed");
            StartCoroutine(OnLeave());
        }
        activeTicks = 3;
    }
    IEnumerator OnLeave(){
        while(activeTicks > 0){
            yield return new WaitForFixedUpdate();
            activeTicks--;
        }
    }
}
