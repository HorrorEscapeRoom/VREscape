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
        private const float EPSILON = 1.0f;

        void ApplyRotationToModel(Transform model, float rotationAmount) =>
        model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.y, rotationAmount, transform.localRotation.eulerAngles.y);

        void ApplyRotationToObject(GameObject targetObject, float rotationAmount)
        {
            if (rotationApplicable) targetObject.transform.Rotate(0, rotationAmount, 0);
        }

        bool ApplyRotation(ref float absAngle, ref float timer, float deltaAngle, Transform model = null, GameObject targetObject = null)
        {
            if (deltaAngle > EPSILON)
            {
                prevAngle = absAngle;
                float rotationAmount = absAngle / timer * Time.deltaTime;
                if (targetObject != null)
                    ApplyRotationToObject(targetObject, rotationAmount);
                else if (model != null)
                    ApplyRotationToModel(model, rotationAmount);
                return true;
            }
            else timer = 0f;
            return false;
        }

        bool IsRotationApplicable(float rotationAmount, Transform model = null, GameObject targetObject = null)
        {
            bool applicable = false;
            if (model != null)
            {
                applicable = Locked;
            }
            if (targetObject != null)
            {
                applicable = targetObject.transform.rotation - initial position door > 180;
            }
            return applicable;
        }
    }
}
