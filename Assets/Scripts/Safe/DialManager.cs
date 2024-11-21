using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Safe
{
    public class DialManager
    {
        public int NumberDialed { get; private set; }
        public float TotalRotations { get; private set; }

        public void UpdateDialState(float currentAngle, float prevAngle, float stepAngle)
        {
            float angleDifference = RotationUtility.CalculateAngleDifference(currentAngle, prevAngle);
            if (angleDifference > 0)
            {
                UpdateNumberDialed(currentAngle, stepAngle);
                UpdateTotalRotations(angleDifference);
            }
        }

        private void UpdateNumberDialed(float currentAngle, float stepAngle)
        {
            NumberDialed = RotationUtility.CalculateDialNumber(currentAngle, stepAngle);
        }

        private void UpdateTotalRotations(float angleDifference)
        {
            TotalRotations += RotationUtility.CalculateTotalRotations(angleDifference);
        }
    }
}
