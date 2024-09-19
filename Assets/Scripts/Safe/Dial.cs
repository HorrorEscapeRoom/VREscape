using UnityEngine;

public class Dial : MonoBehaviour
{
    [SerializeField]
    int _numberSections;

    public float CurrentAngle { get; private set; }
    float previousAngle;
    const float epsilon = 0.001f;  // Tolerance for floating-point precision issues

    DialAngleManager _angleManager = new();
    float angleThreshold = 0.1f;  // Sensitivity threshold for detecting rotation changes

    public int NumberSections
    {
        get => _numberSections;
        set
        {
            if (_numberSections != value)
            {
                //Debug.Log($"NumberSections changed to: {_numberSections} -> {value}");
                _numberSections = value;
                _angleManager.DialSteps = value;
            }
        }
    }

    void Start()
    {
        _angleManager.DialSteps = NumberSections;
        previousAngle = transform.eulerAngles.y; 
        //Debug.Log($"Start: Previous Angle: {previousAngle}, DialSteps: {_angleManager.DialSteps}");
    }

    void Update()
    {
        float newAngle = transform.eulerAngles.z;
        float deltaAngle = NormalizeAngle(newAngle - previousAngle);

        // Only proceed if the delta angle is greater than our defined epsilon tolerance
        if (Mathf.Abs(deltaAngle) > epsilon)
        {
            //Debug.Log($"Update: Previous Angle: {previousAngle}, New Angle: {newAngle}, Delta Angle: {deltaAngle}");

            if (Mathf.Abs(deltaAngle) > angleThreshold)
            {
                CurrentAngle = GetWrappedAngle(newAngle);
                //Debug.Log($"Significant rotation detected. Current Angle: {CurrentAngle}");
                OnRotate(deltaAngle);
            }

            previousAngle = newAngle;
        }
        //Debug.Log($"Update: Small delta angle detected (Delta: {deltaAngle}), ignoring due to epsilon tolerance.");
    }

    void OnRotate(float deltaAngle)
    {
        bool isClockwise = deltaAngle > 0;
        int newDialValue = _angleManager.GetDialIndex(CurrentAngle);

        //Debug.Log($"OnRotate: Rotating {(isClockwise ? "clockwise" : "counter-clockwise")}, Delta Angle: {deltaAngle}, Dial rotated to value {newDialValue}");
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180) angle -= 360;
        else if (angle < -180) angle += 360;

        //Debug.Log($"Normalized Angle: {angle}");
        return angle;
    }

    float GetWrappedAngle(float angle)
    {
        float wrappedAngle = (angle + 360) % 360;
        //Debug.Log($"Raw Angle: {angle}, Wrapped Angle: {wrappedAngle}");
        return wrappedAngle;
    }
}