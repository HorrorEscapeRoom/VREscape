using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Dial : MonoBehaviour
{
    [SerializeField] int[] target;
    [SerializeField] int dialSteps;
    List<int> dialValues = new();
    float currentAngle = 0;

    public void OnRotate(float currentAngle)
    {
        int num = Mathf.FloorToInt(ReScale(currentAngle, 0, 360, 1, dialSteps));
        StartCoroutine(DialTick(num));
    }

    IEnumerator DialTick(int target)
    {
        yield return new WaitForSeconds(0.75f);
        if (target == Mathf.FloorToInt(ReScale(currentAngle, 0, 360, 1, dialSteps)))
        {
            if (dialValues.Count == 0)
            {
                dialValues.Add(target);
            }
            else
            {
                if (dialValues[^1] != target)
                {
                    dialValues.Add(target);
                }
            }

            if (dialValues.Count == this.target.Length)
            {
                bool correct = true;
                for (int i = 0; i < this.target.Length; i++)
                {
                    if (this.target[i] != dialValues[i])
                    {
                        correct = false;
                        break;
                    }
                }

                if (correct)
                {
                    Debug.Log("Correct");

                }
                else
                {
                    Debug.Log("Incorrect!");
                }
                dialValues.Clear();
            }
        }
    }

    float ReScale(float value, float min, float max, float newMin, int newMax) => (value - min) / (max - min) * (newMax - newMin) + newMin;

    void Start()
    {

    }


    void Update()
    {
        
    }
}
