using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.ProBuilder.Shapes;

namespace Assets.Scripts.Safe
{
    public class CombinationSequenceManager : CombinationBaseManager
    {
        int predecessor;
        int timesPredecessorPassed;

        public CombinationSequenceManager(SoundUtility soundUtility, Door door, float stepAngle)
            : base(soundUtility, door, stepAngle) { }

        public void IncrementPredecessorCount(float angleDifference)
        {
            if (currentDigitIndex > predecessor && IsExpectedDirection(angleDifference)) 
                timesPredecessorPassed++;
        }

        public bool IsSequenceProgressionCorrect() => timesPredecessorPassed == currentDigitIndex;
    }
}
