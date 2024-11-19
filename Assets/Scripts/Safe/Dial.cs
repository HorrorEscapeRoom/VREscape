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
    float totalRotations;
    float pauseTimer;

    XRKnob xrKnob;
    DialStateManager stateManager;
    SoundUtility soundUtility;

    [SerializeField] private int amountDialNumbers;
    [SerializeField] private float pauseDuration;

    private float StepAngle => 360f / amountDialNumbers;

    void Start()
    {
        xrKnob = gameObject.AddComponent<XRKnob>();
        stateManager = new DialStateManager();
        soundUtility = gameObject.AddComponent<SoundUtility>();
    }

    void Update() => HandleRotationUpdate();

    // Ensure rotation speed matches the controller's movement.
    // Measure the time taken for `XRKnob.absAngle` to change and calculate the difference 
    // between the previous and current `XRKnob.absAngle` to determine the dial's rotation speed.
    // Capture the controller's displacement on the z-axis and use it to adjust the door's rotation timing.


    public void HandleRotationUpdate()
    {
        float currentAngle = xrKnob.absAngle; // Assuming XRKnob provides this
        float angleDifference = currentAngle - prevAngle;

        if (Mathf.Abs(angleDifference) > EPSILON)
        {
            stateManager.UpdateNumberDialed(currentAngle, StepAngle);
            totalRotations += Mathf.FloorToInt(angleDifference / 360f);
            // Play sound for any rotation
            Debug.Log($"Dialed Number: {stateManager.NumberDialed}");
        }

        prevAngle = currentAngle;
    }
}