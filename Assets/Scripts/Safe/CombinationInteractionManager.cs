using Assets.Scripts.Safe;
using UnityEngine.ProBuilder.Shapes;

public class CombinationInteractionManager : CombinationBaseManager
{
    public int timesPredecessorPassed;

    public CombinationInteractionManager(SoundUtility soundUtility, Door door, float stepAngle)
        : base(soundUtility, door, stepAngle) { }

    public override void ResetCombination()
    {
        base.ResetCombination();
        timesPredecessorPassed = 0;
    }

    public void IncrementPredecessorCount(float angleDifference)
    {
        if (currentDigitIndex > 0 && IsExpectedDirection(angleDifference))
            timesPredecessorPassed++;
    }

    public bool TryAdvanceStep(bool isCorrect, ref float timer, float resetThreshold)
    {
        if (timer >= resetThreshold) timer = 0;

        return isCorrect && IncrementIndex(combination[currentDigitIndex]);
    }
}