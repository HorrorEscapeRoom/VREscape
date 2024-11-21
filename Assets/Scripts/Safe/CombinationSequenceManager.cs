using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.ProBuilder.Shapes;
using static Assets.Scripts.Safe.SoundUtility;

namespace Assets.Scripts.Safe
{
    public class CombinationSequenceManager : CombinationBaseManager
    {
        private int timesPredecessorPassed;

        public CombinationSequenceManager(SoundUtility soundUtility, Door door)
         : base(soundUtility, door) { }

        public override void ResetCombination()
        {
            base.ResetCombination();
            timesPredecessorPassed = 0;
        }

        public void IncrementPredecessorCount(float angleDifference)
        {
            if (currentDigitIndex > 0 && RotationUtility.IsExpectedDirection(angleDifference, currentDigitIndex))
            {
                timesPredecessorPassed++;
            }
        }

        public bool IsSequenceValid() => timesPredecessorPassed == currentDigitIndex;
    }
}