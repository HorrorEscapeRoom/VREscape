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
        public CombinationStateManager(SoundUtility soundUtility, Door door)
            : base(soundUtility, door) { }

        public bool TryAdvanceStep(bool isCorrect, ref float timer, float timeLimit)
        {
            if (timer >= timeLimit)
            {
                timer = 0;
            }

            return isCorrect && IncrementIndex(combination[currentDigitIndex]);
        }
    }
}