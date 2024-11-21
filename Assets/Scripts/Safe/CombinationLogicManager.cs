using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace Assets.Scripts.Safe
{
    public class CombinationLogicManager : CombinationBaseManager
    {
        int totalRotations;

        public CombinationLogicManager(SoundUtility soundUtility, Door door)
            : base(soundUtility, door) { }

        public override void ResetCombination()
        {
            base.ResetCombination();
            totalRotations = 0;
        }

        public int CalculateCorrectRotations()
        {
            if (currentDigitIndex == 0) return 0;

            int currentAngle = combination[currentDigitIndex] * Mathf.RoundToInt(360f / combination.Count);
            int previousAngle = combination[currentDigitIndex - 1] * Mathf.RoundToInt(360f / combination.Count);
            int rotations = Mathf.FloorToInt((currentAngle - previousAngle) / 360f);
            Debug.Log($"Current Angle: {currentAngle}, Previous Angle: {previousAngle}, Rotations: {rotations}");
            return rotations;
        }
    }
}