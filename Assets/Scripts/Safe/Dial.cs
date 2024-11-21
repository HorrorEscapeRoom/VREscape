using UnityEngine;

namespace Assets.Scripts.Safe
{
    public abstract class Dial : MonoBehaviour
    {
        const float EPSILON = 1.0f;
        DialManager dialManager;
        SoundHandler soundHandler;
        XRKnob xRKnob;

        [SerializeField] public int AmountDialNumbers;

        float StepAngle => 360f / AmountDialNumbers;

        void Start()
        {
            dialManager = new DialManager();
            soundHandler = new SoundHandler(SoundUtility.Instance);
            xRKnob = gameObject.AddComponent<XRKnob>();
        }

        void Update() => HandleRotationUpdate();

        private void HandleRotationUpdate()
        {
            float currentAngle = xRKnob.absAngle;
            float prevAngle = dialManager.TotalRotations % 360;
            float angleDifference = RotationUtility.CalculateAngleDifference(currentAngle, prevAngle);

            if (Mathf.Abs(angleDifference) > EPSILON)
            {
                dialManager.UpdateDialState(currentAngle, prevAngle, StepAngle);
                soundHandler.PlayMovingSound();
                Debug.Log($"Dialed Number: {dialManager.NumberDialed}");
            }
        }
    }
}