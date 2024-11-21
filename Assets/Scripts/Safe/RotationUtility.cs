using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RotationUtility
{
    public static float CalculateAngleDifference(float currentAngle, float prevAngle) =>
        Mathf.Abs(currentAngle - prevAngle);

    public static int CalculateTotalRotations(float angleDifference) =>
        Mathf.FloorToInt(angleDifference / 360f);

    public static int CalculateDialNumber(float angle, float stepAngle) =>
        Mathf.FloorToInt(angle / stepAngle);

    public static bool IsEven(int number) => number % 2 == 0;

    public static bool IsExpectedDirection(float angleDiff, int index) =>
        (angleDiff > 0) == IsEven(index);
}
