using UnityEngine;

namespace Assets.Scripts.Safe
{
    public abstract class DoorState : MonoBehaviour
    {
        private bool isOpenable;
        private bool isOpened;

        public void SetOpenable(bool openable) => isOpenable = openable;

        public void UpdateDoorState()
        {
            if (isOpenable && !isOpened)
            {
                isOpened = true;
                Debug.Log("Door opened.");
            }
        }
    }
}
