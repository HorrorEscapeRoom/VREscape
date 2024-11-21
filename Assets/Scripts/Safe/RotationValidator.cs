using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Safe
{
    public abstract class RotationValidator
    {
        public int NecessaryAmount { get; set; }
        public int CurrentAmount { get; set; }
        public string CurrentDirection { get; set; }
        public string ExpectedDirection { get; set; }

        public bool ValidateRotation() =>
            CurrentDirection == ExpectedDirection && CurrentAmount == NecessaryAmount;
    }
}
