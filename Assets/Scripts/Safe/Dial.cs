using Assets.Scripts.Safe;
using System;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.XR;
using static Assets.Scripts.Safe.SoundUtility;

public abstract class Dial : MonoBehaviour
{
    

    const float EPSILON = 1.0f;
    float prevAngle;
    XRKnob XRKnob;
    float absAngle;
    float pauseTimer;
    float stepAngle;
    float totalAmountRotations;
    bool rotationApplicable;
    bool locked;
    int numberDialed;
    readonly SoundUtility soundUtility = new();

    [SerializeField] int amountDialNumbers;
    [SerializeField] float pauseDuration;

    

    void Start()
    {
        XRKnob = new();
        stepAngle = 360 / amountDialNumbers;
    }

    void Update()
    {
        //int number = Mathf.FloorToInt(prevAngle / stepAngle);
        //if (NumberDialed != number) NumberDialed = number;
    }

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

    

    // Doing evrything below so that the speed of rotations match the controller.
    // Could just time the taken for XRKnob.absAngle to change and the diffrence between previous and current XRKnob.absAngle to work out dial's rotation speed.
    // Capture the displacement on the controller's z-axis and the time to rotate the door.
    

    public void HandleRotationUpdate()
    {
        float absAngle = AbsAngle;
        float angleDifference = absAngle - prevAngle;
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