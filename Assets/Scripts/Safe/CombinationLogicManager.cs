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
        private int totalRotations;

        public CombinationLogicManager(SoundUtility soundUtility, Door door, float stepAngle)
            : base(soundUtility, door, stepAngle) { }

        public override void ResetCombination()
        {
            base.ResetCombination();
            totalRotations = 0;
        }

        public int CalculateCorrectRotations()
        {
            if (currentDigitIndex == 0) return 0;
            int currentAngle = combination[currentDigitIndex] * (int)stepAngle;
            int previousAngle = combination[currentDigitIndex - 1] * (int)stepAngle;
            int rotations = Mathf.FloorToInt((currentAngle - previousAngle) / 360);
            Debug.Log($"Current Angle: {currentAngle}, Previous Angle: {previousAngle}, Rotations: {rotations}");
            return rotations;
        }
    }

}