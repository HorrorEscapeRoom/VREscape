using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.XR;

public class Dial : MonoBehaviour
{
    const float EPSILON = 1.0f;
    float prevAngle;  // # dial
    bool locked;
    int numberDialed;
    Door door = new();
    Dial dial = new();
    Combination combination = new();
    Rotations rotations = new();

   
    bool lockSoundPlayed;  // # door
    float doorsInitialYRotation;  // # door
    int totalAmountRotations;  // # dial
   
    float timerHandRotation = 0.0f;  // # dial
    float timerHandDisplacement = 0.0f;  // # dial
    float pauseTimer = 0.0f;  // # dial
    float stepAngle;  // # dial


    [SerializeField] int amountDialNumbers;  // # dial

    [SerializeField] float pauseDuration = new();  // # dial
    [SerializeField] AudioSource[] audioSources;  // # door

    enum Sound
    {
        CorrectNumber,  // # combination
        IncorrectNumber,  // # combination
    }

    public bool Locked
    {
        get => locked;
        set
        {
            if (locked != value)
            {
                locked = value;
            }
        }
    }

    // # door
    void PlaySound(Sound soundType)
    {
        if (!audioSources[(int)soundType].isPlaying)
        {
            audioSources[(int)soundType].Play();
            Debug.Log($"Playing sound: {soundType}");
        }
    }

    // # dial
    void UpdateTimerIfCondition(ref float timer, bool condition)
    {
        if (condition) timer += Time.deltaTime;
        else timer = 0f;
    }

    // # dial
    bool IsExpectedDirection(float angleDiff)
    {
        bool expectedDirection = (angleDiff > 0) == (indexOfCorrectDigit % 2 == 0);
        Debug.Log($"Expected direction: {expectedDirection}, Index: {indexOfCorrectDigit}");
        return expectedDirection;
    }

    // # dial
    bool ApplyRotation(ref float absAngle, ref float timer, float deltaAngle, Transform model = null, GameObject targetObject = null)
    {
        if (deltaAngle > EPSILON)
        {
            prevAngle = absAngle;
            if (targetObject == null)
            {
                model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.x, absAngle, transform.localRotation.eulerAngles.z);
            }
            else targetObject.transform.Rotate(0, absAngle / timer * Time.deltaTime, 0);

            angleOffset = initGrabHandAngel - GetHandThing();
            OnValueChanged?.Invoke(absAngle);
            hud.DrawLine(transform.position, hand.position + hand.up, 0.02f, Color.red);

            Debug.Log($"Applying rotation: AbsAngle={absAngle}, DeltaAngle={deltaAngle}");
            return true;
        }
        else timer = 0f;
        return false;
    }

    // # dial
    void CheckAndApplyRotation(float absAngle, float angleDifference)
    {
        if (ApplyRotation(ref absAngle, ref pauseTimer, angleDifference, model))
        {
            if (AllConditionsMatch(angleDifference))
            {
                ProcessDigitDialed(ref pauseTimer, ref pauseDuration, prevAngle, absAngle);
                IncrementPredecessorCount(ref timesPredecessorPassed, angleDifference);
            }
            totalAmountRotations = Mathf.FloorToInt((angleDifference) / 360.0f);
            Debug.Log($"Checking rotation: AbsAngle={absAngle}, Angle Difference={angleDifference}");
        }
    }

    // # dial
    public void HandleRotationUpdate()
    {
        float absAngle = AbsAngle;
        float angleDifference = absAngle - prevAngle;
        if (active)
        {
            if (!IsHandNearby())
            {
                Released();
                return;
            }
            else Grabbed(hand);
            if (UpdateDoorState())  // # door
            {
                handPositionZ = hand.transform.position.z;
                needReset = true;  // # combination
                return;
            }
            else CheckAndApplyRotation(absAngle, angleDifference);  // # dial
            if (needReset) ResetCombination(ref prevAngle, ref absAngle);  // # combination
            if (canDoorOpen)  // # door
            {
                HandleDoorRotation();  // # door
            }
        }
    }

    // Start is called before the first frame update
    // # dial, # door
    void Start()
    {
        try
        {
            stepAngle = 360 / amountDialNumbers;  // # dial
        }
        catch (DivideByZeroException)
        {
            Debug.Log("Amount of dial numbers is missing.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        numberDialed = ...
    }
}
