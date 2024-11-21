using System;

namespace Assets.Scripts.Safe
{
    public class DoorManager
    {
        readonly DoorState doorState;
        readonly ICombinationStatusProvider combinationStatusProvider;

        public DoorManager(DoorState doorState, ICombinationStatusProvider combinationStatusProvider)
        {
            this.doorState = doorState != null ? doorState : throw new ArgumentNullException(nameof(doorState));
            this.combinationStatusProvider = combinationStatusProvider ?? throw new ArgumentNullException(nameof(combinationStatusProvider));
        }

        public void UpdateDoorAccess()
        {
            if (combinationStatusProvider.Completed)
                doorState.SetOpenable(true);
        }
    }
}