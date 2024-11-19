using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Safe
{
    public static class DialCalculator
    {
        public static int CalculateDialNumber(float angle, float stepAngle) => Mathf.FloorToInt(angle / stepAngle);
    }
}
