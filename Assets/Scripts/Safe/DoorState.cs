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
        //protected const float MaxAngleIncrement = 180f;
        //protected bool isDoorPreviouslyOpened = false;
        //protected float initialPosition;
        //protected float maxAngle;
        //protected float previousAngle;
        //bool opened = false;
        //readonly SoundUtility soundUtility = new();
        //DoorManager manager;

        //bool IsOpen { get; set; } = false;
        SoundHandler soundHandler;
        bool isOpenable;

        public bool IsOpenable
        {
            get => isOpenable;
            private set => isOpenable = value;
        }

        public void SetOpenable(bool openable) => IsOpenable = openable;

        public void InitializeDoor(GameObject door, SoundHandler soundHandler)
        {
            this.soundHandler = soundHandler;
            initialPosition = door.transform.eulerAngles.y;
            maxAngle = initialPosition + MaxAngleIncrement;
            previousAngle = initialPosition;
        }

        public void UpdateDoorState(float currentAngle)
        {
            if (IsOpen)
            {
                if (!opened) opened = true;
            }
            if (!IsOpen && opened)
            {
                IsOpenable = false;
                opened = false;
                soundHandler.PlayCorrectSound();
            }
            if (currentAngle != previousAngle)
            {
                previousAngle = currentAngle;
                soundHandler.PlayIncorrectSound();
            }
        }

        public void Start() => previousAngle = initialPosition;
    }
}
