using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Responsible for its state and behaviour
public class Disk : MonoBehaviour, IDisk
{
    Vector3 lastPosition;
    public float detectionRadius = 0.10f;
    GameObject otherDisk;

    public Vector3 LastPosition
    {
        get => lastPosition;
        set
        {
            if (lastPosition != value)
            {
                lastPosition = value;
                DiskManager.UpdateDiskPosition(gameObject.name, value);
            }
        }
    }

    public bool IsOtherDiskLarger => otherDisk != null &&
        otherDisk.GetComponent<CapsuleCollider>().radius > gameObject.GetComponent<CapsuleCollider>().radius;

    public void SnapToOtherDisk()
    {
        if (otherDisk != null && IsOtherDiskLarger && DiskManager.IsAfter(gameObject, otherDisk))
        {
            Vector3 newPosition = new(
                otherDisk.transform.position.x,
                CalculateSnappedYPosition(),
                otherDisk.transform.position.z
                );
            transform.position = newPosition;
            LastPosition = newPosition;
        }
    }

    private float CalculateSnappedYPosition()
    {
        float currentHeight = GetComponent<CapsuleCollider>().height;
        float otherHeight = otherDisk.GetComponent<CapsuleCollider>().height;
        return otherDisk.transform.position.y + (currentHeight / 2) + (otherHeight / 2);
    }

    void Start()
    {
        DiskManager.AddDisk(gameObject);
    }

    void Update()
    {
        LastPosition = transform.position;
    }

    // works 
    void OnTriggerEnter(Collider other)
    {
        Disk disk = other.GetComponent<Disk>();
        if (disk != null)
        {
            otherDisk = other.gameObject;
        }
        SnapToOtherDisk();
    }
}