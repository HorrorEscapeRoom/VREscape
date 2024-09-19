using UnityEngine;

public class DialSection : MonoBehaviour
{
    DialAngleManager _angleManager = new();
    public float CurrentAngle { get; set; }

    public int DialSteps
    {
        get => _angleManager.DialSteps;
        set
        {
            if (_angleManager.DialSteps != value)
            {
                //Debug.Log($"DialSteps in DialSection changed to: {_angleManager.DialSteps} -> {value}");
                _angleManager.DialSteps = value;
            }
        }
    }

    public int GetDialIndex()
    {
        int index = _angleManager.GetDialIndex(CurrentAngle);
        // Debug.Log($"DialSection GetDialIndex: Current Angle: {CurrentAngle}, Dial Index: {index}");
        return index;
    }

    public void CalculateDialAngles()
    {
        var ranges = _angleManager.CalculateDialRanges();
        //foreach (var range in ranges)
        //{
        //    Debug.Log($"Dial Angle Range: {range[0]} to {range[1]}");
        //}
    }
}