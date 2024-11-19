using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Safe
{
    public class RotationCalculator
    {
        [SerializeField] private float rotationTimer = 0f;
        public float CalculateAngleDifference(float currentAngle, float prevAngle) => Mathf.Abs(currentAngle - prevAngle);

        public float CalculateRotationAmount(float absAngle, float timer)
        {
            if (timer <= 0f) throw new ArgumentException("Timer must be greater than 0.", nameof(timer));
            return absAngle / timer * Time.deltaTime;
        }

        public float RotationTimer
        {
            get => rotationTimer;
            set
            {
                if (value <= 0f)
                {
                    throw new ArgumentException("RotationTimer must be greater than 0.");
                }
                rotationTimer = value;
            }
        }
    }
}
