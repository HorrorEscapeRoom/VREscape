using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace Assets.Scripts.Safe
{
    public class CombinationInteractionManager : CombinationBaseManager
    {
        public int TimesPredecessorPassed { get; private set; }

        public CombinationInteractionManager(SoundUtility soundUtility, Door door)
            : base(soundUtility, door) { }

        public override void ResetCombination()
        {
            base.ResetCombination();
            TimesPredecessorPassed = 0;
        }

        public void IncrementPredecessorCount(float angleDifference)
        {
            if (currentDigitIndex > 0 && RotationUtility.IsExpectedDirection(angleDifference, currentDigitIndex))
                TimesPredecessorPassed++;
        }

        public bool TryAdvanceStep(bool isCorrect, ref float timer, float resetThreshold)
        {
            if (timer >= resetThreshold) timer = 0;
            return isCorrect && IncrementIndex(combination[currentDigitIndex]);
        }
    }
}