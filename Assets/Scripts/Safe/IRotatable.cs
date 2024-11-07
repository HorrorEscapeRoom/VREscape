using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRotatable 
{
    void Rotate(int rotate);
    float CurrentAngle { get; }
}
