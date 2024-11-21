using System;
using UnityEngine;

namespace Assets.Scripts.Safe
{
    public abstract class RotationValidator
    {
        public int NecessaryAmount { get; set; }
        public int CurrentAmount { get; set; }
        public string CurrentDirection { get; set; }
        public string ExpectedDirection { get; set; }

        public bool ValidateRotation()
        {
            bool isValid = CurrentDirection == ExpectedDirection && CurrentAmount == NecessaryAmount;
            Debug.Log($"[RotationValidator] ValidateRotation() = {isValid}");
            return isValid;
        }
    }
}