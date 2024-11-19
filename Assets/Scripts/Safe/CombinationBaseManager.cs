using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace Assets.Scripts.Safe
{
    public abstract class CombinationBaseManager : MonoBehaviour, ICombinationStatusProvider
    {
        [SerializeField]
        protected List<int> combination;

        protected int currentDigitIndex;
        protected SoundUtility soundUtility;
        protected Door door;
        protected float stepAngle;

        protected CombinationBaseManager(SoundUtility soundUtility, Door door, float stepAngle)
        {
            this.soundUtility = soundUtility;
            this.door = door;
            this.stepAngle = stepAngle;
            ResetCombination();
        }

        public void ResetCombination() => currentDigitIndex = 0;

        public bool IsNumberCorrect(int numberDialed) => numberDialed == combination[currentDigitIndex]; 

        protected bool IncrementIndex(int numberDialed)
        {
            if (IsNumberCorrect(numberDialed))
            {
                currentDigitIndex++;
                return Completed;
            }
            return false;
        }

        protected bool IsExpectedDirection(float angleDiff) => (angleDiff > 0) == (currentDigitIndex % 2 == 0);

        public bool Completed
        {
            get => currentDigitIndex == combination.Count - 1 && IsNumberCorrect(combination[currentDigitIndex - 1]);
            // predecessor needs to be passed a certain amount of times and the number of rotations needs to be correct,
            // any deviation needs to cause a restart
        }

        protected int GetPredecessor() => combination[(currentDigitIndex - 1 + combination.Count) % combination.Count];

        public bool IncrementIndexAndResetTimer(bool isCorrect, ref float timer, int numberDialed)
        {
            if (isCorrect && IncrementIndex(numberDialed))
            {
                timer = 0;
                return true;
            }
            return false;
        }

        public void IncrementPredecessorCount(float angleDifference, int predecessor, ref int timesPredecessorPassed)
        {
            if (currentDigitIndex > predecessor && IsExpectedDirection(angleDifference))
            {
                timesPredecessorPassed++;
            }
        }

        //public int CalculateCorrectRotations()
        //{
        //    int currentAngle = combination[currentDigitIndex] * (int)stepAngle;
        //    int previousAngle = combination[Mathf.Max(currentDigitIndex - 1, 0)] * (int)stepAngle;
        //    float angleDifference = currentAngle - previousAngle;
        //    return Mathf.FloorToInt(angleDifference / 360);
        //}
    }
}
