using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.Shapes;
using static Assets.Scripts.Safe.SoundUtility;

namespace Assets.Scripts.Safe
{
    public class CombinationStateManager : CombinationBaseManager
    {
        public CombinationStateManager(SoundUtility soundUtility, Door door, float stepAngle)
        : base(soundUtility, door, stepAngle) { }

        public bool TryAdvanceStep(bool isCorrect, ref float timer, float timeLimit)
        {
            if (timer >= timeLimit)
            {
                timer = 0;
            }

            if (isCorrect && IncrementIndex(combination[currentDigitIndex]))
            {
                soundUtility.PlaySound(SoundType.CorrectNumber);
                return true;
            }

            return false;
        }
    }
}