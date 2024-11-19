using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Assets.Scripts.Safe.SoundUtility;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine;

namespace Assets.Scripts.Safe
{
    public class CombinationInteractionManager
    {
        // Handles interactions between the dial and the combination system.
        readonly CombinationBaseManager combinationManager;

        public CombinationInteractionManager(CombinationBaseManager combinationBaseManager)
        {
            combinationManager = combinationBaseManager != null ? combinationBaseManager : throw new ArgumentNullException(nameof(combinationBaseManager), "CombinationBaseManager cannot be null.");
        }

        public void ResetCombination() => combinationManager.ResetCombination();  
        
        public bool ProcessDialedNumber(int numberDialed, ref float timer)
        {
            bool isCorrect = combinationManager.IsNumberCorrect(numberDialed);
            return combinationManager.IncrementIndexAndResetTimer(isCorrect, ref timer, numberDialed);
        }

        public void UpdatePredecessorCount(float angleDifference)
        {
            if (combinationManager is CombinationSequenceManager sequenceManager)
            {
                sequenceManager.IncrementPredecessorCount(angleDifference);
            }
        }

        public bool IsSequenceProgressionCorrect()
        {
            if (combinationManager is CombinationSequenceManager sequenceManager)
            {
                return sequenceManager.IsSequenceProgressionCorrect();
            }
            return false;
        }

        // original function
        //public void ProcessDigitDialed(ref float timer, ref float pauseDuration, float absAngle)
        //{
        //    if (locked) return;

        //    if (timer >= pauseDuration)
        //    {
        //        NumberDialed = Mathf.FloorToInt(absAngle / stepAngle);
        //        //correctAmountRotations = CalculateCorrectRotations();

        //        if (IncrementIndexOfCorrectDigit(NumberDialed, ref timer))
        //        {
        //            Locked = true;
        //            soundUtility.PlaySound(SoundType.CorrectNumber);
        //            door.Openable = true;
        //            PlaySound(Sound.Unlocking);
        //        }
        //        else soundUtility.PlaySound(SoundType.IncorrectNumber);
        //    }
        //}
    }
}
