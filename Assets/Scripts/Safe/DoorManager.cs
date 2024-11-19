using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Safe
{
    public class DoorManager
    {
        readonly DoorState doorState;
        readonly ICombinationStatusProvider combinationStatusProvider;

        public DoorManager(DoorState doorState, ICombinationStatusProvider combinationStatusProvider)
        {
            this.doorState = doorState != null ? doorState : throw new ArgumentNullException(nameof(doorState), "doorState is null during initialization of DoorManager.");
            this.combinationStatusProvider = combinationStatusProvider ?? throw new ArgumentNullException(nameof(combinationStatusProvider), "combinationStatusProvider is null during initialization of DoorManager.");
        }

        public void MakeDoorOpenable()
        {
            if (combinationStatusProvider.Completed) doorState.SetOpenable(true);
        }

        public void LockDoor() => doorState.SetOpenable(false);

        public bool IsDoorOpenable() => doorState.IsOpenable;
    }
}
