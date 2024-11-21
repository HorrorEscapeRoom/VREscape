using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace Assets.Scripts.Safe
{
    public abstract class CombinationBaseManager : MonoBehaviour
    {
        [SerializeField] protected List<int> combination;
        protected int currentDigitIndex;
        protected SoundUtility soundUtility;
        protected Door door;

        protected CombinationBaseManager(SoundUtility soundUtility, Door door)
        {
            this.soundUtility = soundUtility != null ? soundUtility : throw new ArgumentNullException(nameof(soundUtility));
            this.door = door ?? throw new ArgumentNullException(nameof(door));
            ResetCombination();
        }

        public virtual void ResetCombination() => currentDigitIndex = 0;

        public bool IsNumberCorrect(int numberDialed) =>
            currentDigitIndex >= 0 && currentDigitIndex < combination.Count &&
            numberDialed == combination[currentDigitIndex];

        public bool IsCombinationComplete =>
            currentDigitIndex == combination.Count - 1 && IsNumberCorrect(combination[currentDigitIndex]);

        protected bool IncrementIndex(int numberDialed)
        {
            if (!IsNumberCorrect(numberDialed)) return false;

            if (currentDigitIndex < combination.Count - 1)
                currentDigitIndex++;

            return IsCombinationComplete;
        }
    }
}