using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Safe
{
    public class DialStateManager
    {
        public int NumberDialed { get; private set; }
        public bool Locked { get; set; }

        public void UpdateNumberDialed(float prevAngle, float stepAngle)
        {
            int number = DialCalculator.CalculateDialNumber(prevAngle, stepAngle);
            if (NumberDialed != number) NumberDialed = number;
        }
    }
}
