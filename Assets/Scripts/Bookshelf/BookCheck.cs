using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class ObjectNameChecker : PuzzleBase
{
    // Define the four objects
    public GameObject BookObject1;
    public GameObject BookObject2;
    public GameObject BookObject3;
    public GameObject BookObject4;
    public GameObject BookObject5;
    public GameObject BookObject6;

    // Define the four objects
    public GameObject ShelfSpot1;
    public GameObject ShelfSpot2;
    public GameObject ShelfSpot3;
    public GameObject ShelfSpot4;
    public GameObject ShelfSpot5;
    public GameObject ShelfSpot6;

    // Store whether the names are correct
    public bool object1Correct;
    public bool object2Correct;
    public bool object3Correct;
    public bool object4Correct;
    public bool object5Correct;
    public bool object6Correct;

    public Animation animation;
    public bool animationPlayed = false;

    void Start()
    {
        RegisterWithOrchestrator();
    }

    private void FixedUpdate()
    {
        // Check if the names match and set the boolean flags

        if (ShelfSpot1.GetComponent<StorageVolume>().ReadItem() == BookObject1) { object1Correct = true; };
        if (ShelfSpot2.GetComponent<StorageVolume>().ReadItem() == BookObject2) { object2Correct = true; };
        if (ShelfSpot3.GetComponent<StorageVolume>().ReadItem() == BookObject3) { object3Correct = true; };
        if (ShelfSpot4.GetComponent<StorageVolume>().ReadItem() == BookObject4) { object4Correct = true; };
        if (ShelfSpot5.GetComponent<StorageVolume>().ReadItem() == BookObject5) { object5Correct = true; };
        if (ShelfSpot6.GetComponent<StorageVolume>().ReadItem() == BookObject6) { object6Correct = true; };

        if(object1Correct && object2Correct && object3Correct && object4Correct && object5Correct && object6Correct && !animationPlayed)
        {
            animationPlayed = true;
            animation.Play();
            OnPuzzleComplete(true);
        }
    }
}
