using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.Shapes;

namespace Assets.Scripts.Safe
{
    public class CombinationStateManager : CombinationBaseManager
    {
        int numberDialed;

        public CombinationStateManager(SoundUtility soundUtility, Door door, float stepAngle)
            : base(soundUtility, door, stepAngle)
        {
        }

        public bool IncrementIndexAndResetTimer(bool isCorrect, ref float timer) =>
            IncrementIndexAndResetTimer(isCorrect, ref timer, numberDialed);
    }
}