using System;
using UnityEngine;

namespace Assets.Scripts.Safe
{
    public class RotationCalculator
    {
        public float CalculateAngleDifference(float currentAngle, float prevAngle) =>
            Mathf.Abs(currentAngle - prevAngle);

        public float CalculateRotationAmount(float absAngle, float timer)
        {
            if (timer <= 0f)
                throw new ArgumentException("Timer must be greater than 0.", nameof(timer));

            return absAngle / timer * Time.deltaTime;
        }
    }
}
