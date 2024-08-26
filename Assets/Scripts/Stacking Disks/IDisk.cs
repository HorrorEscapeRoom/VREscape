using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDisk
{
    Vector3 LastPosition { get; set; }
    bool IsOtherDiskLarger { get; }
    void SnapToOtherDisk();
}