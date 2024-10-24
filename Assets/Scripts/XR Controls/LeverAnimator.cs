using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverAnimator : MonoBehaviour
{
    [SerializeField] Transform lever;
    [SerializeField] float minAngle, maxAngle;
    public void AnimateLever(float value){
        lever.localEulerAngles = new Vector3(0,0,Mathf.Lerp(minAngle,maxAngle,value));
    }
}
