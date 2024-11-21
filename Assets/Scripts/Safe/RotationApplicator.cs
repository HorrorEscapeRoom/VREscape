using System;
using UnityEngine;

namespace Assets.Scripts.Safe
{
    public class RotationApplicator
    {
        public void ApplyRotation(GameObject targetObject, Transform model, float rotationAmount)
        {
            if (targetObject != null)
                ApplyRotationToObject(targetObject, rotationAmount);
            else if (model != null)
                ApplyRotationToModel(model, rotationAmount);
            else
                throw new ArgumentNullException("Both targetObject and model cannot be null.");
        }

        private void ApplyRotationToObject(GameObject targetObject, float rotationAmount)
        {
            targetObject.transform.Rotate(0, rotationAmount, 0);
        }

        private void ApplyRotationToModel(Transform model, float rotationAmount)
        {
            model.localRotation = Quaternion.Euler(
                model.localRotation.eulerAngles.x,
                rotationAmount,
                model.localRotation.eulerAngles.z
            );
        }
    }
}