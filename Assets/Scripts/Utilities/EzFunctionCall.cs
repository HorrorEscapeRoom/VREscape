using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EzFunctionCall : MonoBehaviour
{
    public UnityEvent events;
    public void CallFunction(){
        events?.Invoke();
    }
}
