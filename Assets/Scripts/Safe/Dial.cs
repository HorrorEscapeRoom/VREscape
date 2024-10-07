using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    [SerializeField]
    int _numberSections = 12; 
    public float CurrentAngle { get; private set; }
    private XRKnob knob;

    public int NumberSections
    {
        get => _numberSections;
        set
        {
            if (_numberSections != value)
            {
                _numberSections = value;
            }
        }
    }

    public float StepAngle() => 360f / NumberSections;

    void Start()
    {
        knob = GetComponentInParent<XRKnob>();
        if (knob == null)
        {
            return;
        }
        knob.OnValueChanged.AddListener(UpdateDialPosition);
    }

    int CalculateDialNumber(float angle) => Mathf.FloorToInt(angle / StepAngle()) % NumberSections;

    public void UpdateDialPosition(float newAngle)
    {
        CurrentAngle = newAngle;
        int dialNumber = CalculateDialNumber(CurrentAngle);
        Debug.Log($"Dial turned to number {dialNumber} with angle {CurrentAngle}");
    }

    //int OnRotate(float angle)
    //{
    //    int stepAngle = StepAngle();
    //    return Mathf.FloorToInt(angle / stepAngle);
    //}

    // void Start() => previousAngle = TransformUtils.GetInspectorRotation(transform).x;

    //void Update()
    //{
    //    int dialIndex = 0;
    //    float newAngle = TransformUtils.GetInspectorRotation(transform).x;
    //    float deltaAngle = newAngle - previousAngle; //  delta angle is change in rotation


    //    //Only proceed if the delta angle is greater than the epsilon tolerance
    //    if (Mathf.Abs(deltaAngle) > EPSILON)
    //    {
    //        CurrentAngle = newAngle % 360; 
    //        Debug.Log($"Delta Angle = {deltaAngle}, New Angle = {newAngle}, True Angle = {CurrentAngle}, Previous Angle = {previousAngle}");
    //        dialIndex = OnRotate(CurrentAngle);
    //        previousAngle = newAngle;
    //    }
    //}

    //int OnRotate(float deltaAngle)
    //{
    //    int stepAngle = StepAngle();
    //    int dialTurned = Mathf.Abs(Mathf.FloorToInt(deltaAngle / stepAngle));
    //    if ((deltaAngle % stepAngle) > 0)
    //    {
    //        dialTurned += 1;
    //    }
    //    Debug.Log($"Rotating, Delta Angle: {deltaAngle},  Dial rotated to value {dialTurned}");
    //    return dialTurned;
    //}
}