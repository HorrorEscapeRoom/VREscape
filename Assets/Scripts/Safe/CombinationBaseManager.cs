using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace Assets.Scripts.Safe
{
    public abstract class CombinationBaseManager : MonoBehaviour
    {
        [SerializeField]
        protected List<int> combination;

        protected int currentDigitIndex;
        protected SoundUtility soundUtility;
        protected Door door;
        protected float stepAngle;

        protected CombinationBaseManager(SoundUtility soundUtility, Door door, float stepAngle)
        {
            this.soundUtility = soundUtility != null ? soundUtility : throw new ArgumentNullException(nameof(soundUtility));
            this.door = door ?? throw new ArgumentNullException(nameof(door));
            this.stepAngle = stepAngle;
            ResetCombination();
        }

        public virtual void ResetCombination() => currentDigitIndex = 0;

        public bool IsNumberCorrect(int numberDialed) =>
            IsWithinBounds(currentDigitIndex) && numberDialed == combination[currentDigitIndex];

        public bool IsCombinationComplete =>
            currentDigitIndex == combination.Count - 1 && IsNumberCorrect(combination[currentDigitIndex]);

        protected bool IncrementIndex(int numberDialed)
        {
            if (!IsNumberCorrect(numberDialed)) return false;

            if (currentDigitIndex < combination.Count - 1)
                currentDigitIndex++;

            return IsCombinationComplete;
        }

        protected bool IsExpectedDirection(float angleDiff) =>
            (angleDiff > 0) == IsEven(currentDigitIndex);

        static bool IsEven(int number) => number % 2 == 0;

        bool IsWithinBounds(int index) => index >= 0 && index < combination.Count;
    }
}