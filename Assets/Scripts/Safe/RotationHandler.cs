using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Safe
{
    public class RotationHandler
    {
        const float EPSILON = 1.0f;
        readonly RotationApplicator applicator;
        readonly RotationCalculator calculator;
        readonly RotationValidator validator;

        public RotationHandler(RotationApplicator applicator, RotationCalculator calculator, RotationValidator validator)
        {
            this.applicator = applicator ?? throw new ArgumentNullException(nameof(applicator));
            this.calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public bool HandleRotation(Transform model, GameObject targetObject, float absAngle, ref float timer, float prevAngle)
        {
            float deltaAngle = calculator.CalculateAngleDifference(absAngle, prevAngle);

            if (deltaAngle > EPSILON)
            {
                validator.CurrentAmount++;
                if (validator.ValidateRotation())
                {
                    float rotationAmount = calculator.CalculateRotationAmount(absAngle, timer);
                    applicator.ApplyRotation(targetObject, model, rotationAmount);
                    return true;
                }
            }
            else timer = 0f;
            return false;
        }
    }
}
