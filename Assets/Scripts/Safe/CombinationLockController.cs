using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace Assets.Scripts.Safe
{
    /// <summary>
    /// Handles the overall behavior, initializes components, and ties evrything together.
    /// </summary>
    public class CombinationLockController : MonoBehaviour
    {
        [SerializeField] CombinationStateManager combinationStateManager;
        [SerializeField] Dial dial;
        [SerializeField] DoorState doorState;

        private DialManager dialManager;
        private SoundHandler soundHandler;
        private float resetTimer;
        [SerializeField] private float resetThreshold = 2f;
        private float prevAngle;

        void Start()
        {
            ValidateComponents();
            soundHandler = new SoundHandler(SoundUtility.Instance);
            dialManager = new DialManager();
        }

        void Update()
        {
            HandleDialRotation();
            HandleCombinationState();
            HandleReset();
        }

        private void ValidateComponents()
        {
            if (combinationStateManager == null || dial == null || doorState == null)
            {
                Debug.LogError("Missing required components for CombinationLockController.");
                enabled = false;
            }
        }

        private void HandleDialRotation()
        {
            float currentAngle = dial.GetComponent<XRKnob>().absAngle;
            dialManager.UpdateDialState(currentAngle, prevAngle, 360f / dial.AmountDialNumbers);
            prevAngle = currentAngle;
        }

        private void HandleCombinationState()
        {
            bool isCorrect = combinationStateManager.IsNumberCorrect(dialManager.NumberDialed);

            if (combinationStateManager.TryAdvanceStep(isCorrect, ref resetTimer, resetThreshold))
            {
                doorState.SetOpenable(true);
                soundHandler.PlayCorrectSound();
            }
        }

        private void HandleReset()
        {
            if (resetTimer > resetThreshold)
            {
                combinationStateManager.ResetCombination();
                resetTimer = 0f;
            }
        }
    }