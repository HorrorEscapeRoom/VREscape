using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;
using static Assets.Scripts.Safe.SoundUtility;

namespace Assets.Scripts.Safe
{
    public abstract class DoorState : MonoBehaviour
    {
        bool isOpenable;
        float initialPosition;
        float maxAngle;
        float previousAngle;
        bool isOpened;
        SoundHandler soundHandler;

        public bool IsOpenable
        {
            get => isOpenable;
            private set => isOpenable = value;
        }

        public void Initialize(GameObject door, SoundHandler soundHandler, float maxAngleIncrement)
        {
            this.soundHandler = soundHandler ?? throw new ArgumentNullException(nameof(soundHandler));
            initialPosition = door.transform.eulerAngles.y;
            maxAngle = initialPosition + maxAngleIncrement;
            previousAngle = initialPosition;
            isOpened = false;
        }

        public void SetOpenable(bool openable) => IsOpenable = openable;

        public void UpdateDoorState(float currentAngle)
        {
            if (currentAngle != previousAngle) previousAngle = currentAngle;
            if (IsOpenable) HandleOpeningState();
            else soundHandler.PlayIncorrectSound();
        }

        void HandleOpeningState()
        {
            if (!isOpened)
            {
                isOpened = true;
                soundHandler.PlayCorrectSound();
            }
            if (isOpened)
            {
                isOpened = false;
                isOpenable = false;
            }
        }
    }
}
