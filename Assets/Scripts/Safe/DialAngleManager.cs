using System.Collections.Generic;
using UnityEngine;

public class DialAngleManager
{
    int _dialSteps;

    public int DialSteps
    {
        get => _dialSteps;
        set
        {
            if (_dialSteps != value)
            {
                Debug.Log($"DialSteps changed to: {_dialSteps} -> {value}");
                _dialSteps = value;
            }
        }
    }

    public int GetDialIndex(float curAngle)
    {
        float sectionSize = 360f / _dialSteps;  // size of each section
        curAngle = GetWrappedAngle(curAngle);  // ensure angle is wrapped to [0, 360)

        // Calculate the index by dividing the angle by section size
        int index = Mathf.FloorToInt(curAngle / sectionSize) + 1;

        // Optional: log the ranges for each section
        Debug.Log($"Angle: {curAngle}, Section Size: {sectionSize}, Index: {index}");

        return index;
    }

    public bool IsRotatedClockwise(float previousAngle, float currentAngle)
    {
        previousAngle = GetWrappedAngle(previousAngle);
        currentAngle = GetWrappedAngle(currentAngle);

        bool isClockwise = (currentAngle > previousAngle && currentAngle - previousAngle < 180) ||
                           (currentAngle < previousAngle && previousAngle - currentAngle > 180);

        //Debug.Log($"Previous Angle: {previousAngle}, Current Angle: {currentAngle}, Is Rotated Clockwise: {isClockwise}");
        return isClockwise;
    }

    public List<int[]> CalculateDialRanges()
    {
        List<int[]> ranges = new();
        float sectionSize = 360f / _dialSteps;
        for (int i = 0; i < _dialSteps; i++)
        {
            int minAngle = Mathf.FloorToInt(i * sectionSize);
            int maxAngle = Mathf.FloorToInt(minAngle + sectionSize);
            ranges.Add(new int[] { minAngle, i == _dialSteps - 1 ? 360 : maxAngle });
            //Debug.Log($"Dial Range {i + 1}: {minAngle}° - {maxAngle}°");
        }
        return ranges;
    }

    public float GetWrappedAngle(float angle)
    {
        float wrappedAngle = (angle + 360) % 360;
        //Debug.Log($"Raw Angle: {angle}, Wrapped Angle: {wrappedAngle}");
        return wrappedAngle;
    }
}