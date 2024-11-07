using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotations : MonoBehaviour
{
    float currentAmount;
    int correctAmount;
    string currentDirection;
    string correctDirection;

    public int CorrectAmount
    {
        set
        {
            if (correctAmount != value)
            {
                correctAmount = value;
            }
        }
        get => correctAmount;
    }

    public float CurrentAmount
    {
        set
        {
            if (currentAmount != value)
            {
                currentAmount = value;
            }
        }
        get => currentAmount;
    }

    public string CurrentDirection
    {
        set
        {
            if (currentDirection != value)
            {
                currentDirection = value;
            }
        }
        get => currentDirection;
    }

    public string CorrectDirection
    {
        set
        {
            if (correctDirection != value)
            {
                correctDirection = value;
            }
        }
        get => correctDirection;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
