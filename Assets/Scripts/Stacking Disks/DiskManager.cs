using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Android;

// Manages disks' positions and order
public class DiskManager : MonoBehaviour
{
    static readonly Dictionary<string, Vector3> positionsDisks = new();
    public static List<GameObject> correctDiskOrder = new();

    // disks are added
    public static void AddDisk(GameObject disk)
    {
        if (!correctDiskOrder.Contains(disk))
        {
            correctDiskOrder.Add(disk);
            SortDisksBySize();
        }
    }

    static void SortDisksBySize()
    {
        correctDiskOrder = correctDiskOrder
             .OrderByDescending(disk => disk.GetComponent<CapsuleCollider>().radius)
             .ToList();
    }

    public static bool IsAfter(GameObject disk, GameObject other)
    {
        int indexDisk = correctDiskOrder.IndexOf(disk);
        int indexOther = correctDiskOrder.IndexOf(other);
        Debug.Log($"{disk.name} after {other.name}: {indexDisk == indexOther + 1}");
        return indexDisk == indexOther + 1;
    }

    public static void UpdateDiskPosition(string diskName, Vector3 position)
    {
        positionsDisks[diskName] = position;
    } 

    public static Vector3 GetDiskPosition(string diskName) => positionsDisks[diskName];
}