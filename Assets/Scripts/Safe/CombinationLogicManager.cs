using OpenCover.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Safe
{
    public class CombinationLogicManager : CombinationBaseManager
    {
        int predecessor;
        int correctAmountRotations;
        int timesPredecessorPassed;
        int totalAmountRotations;
        bool lockDial;
        float handPositionZ;

        //private int currentDigitIndex;
        //private int numberDialed;
        

        //private SoundUtility soundUtility;
        //private Door door;
        //private float stepAngle;

        public CombinationLogicManager(List<int> combination, SoundUtility soundUtility, Door door, float stepAngle)
            : base(soundUtility, door, stepAngle)
        {
        }

        //public CombinationLogicManager(List<int> combination, SoundUtility soundUtility, Door door, float stepAngle)
        //{
        //    this.combination = combination;
        //    this.soundUtility = soundUtility;
        //    this.door = door;
        //    this.stepAngle = stepAngle;
        //    ResetCombination();
        //}

        public void ResetCombination()
        {
            base.ResetCombination();
            totalAmountRotations = 0;
            correctAmountRotations = 0;
            timesPredecessorPassed = 0;
        }

        public int CalculateCorrectRotations()
        {
            int currentAngle = combination[currentDigitIndex] * (int)stepAngle;
            int previousAngle = combination[Mathf.Max(currentDigitIndex - 1, 0)] * (int)stepAngle;
            float angleDifference = currentAngle - previousAngle;
            int rotations = Mathf.FloorToInt(angleDifference / 360);
            Debug.Log($"Current Angle: {currentAngle}, Previous Angle: {previousAngle}, Calculated Rotations: {rotations}");
            return rotations;
        }

        public bool IncrementIndexOfCorrectDigit(bool numberDialedCorrect, ref float timer)
        {
            if (numberDialedCorrect)
            {
                soundUtility.PlaySound(SoundUtility.SoundType.CorrectNumber);
                predecessor = combination[(currentDigitIndex - 1 + combination.Count) % combination.Count];
                if (currentDigitIndex != combination.Count - 1)
                {
                    currentDigitIndex++;
                    Debug.Log($"Index of correct digit incremented: {currentDigitIndex}");
                    timer = 0;
                    return true;
                }
            }
            return false;
        }

        public void IncremenrPredecessorCount(float angleDifference) => 
            IncrementPredecessorCount(angleDifference, predecessor, ref timesPredecessorPassed);

        //public bool IsDoorAndLockStateValid()
        //{
        //    return !door.Opened && !lockDial && !door.Openable;
        //}

        //public void UpdateHandPositionIfDoorClosed(Transform hand)
        //{
        //    if (!door.Openable)
        //    {
        //        handPositionZ = hand.position.z;
        //    }
        //}
    }
}