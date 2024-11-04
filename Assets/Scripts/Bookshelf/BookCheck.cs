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

    // Define the four objects
    public GameObject ShelfSpot1;
    public GameObject ShelfSpot2;
    public GameObject ShelfSpot3;
    public GameObject ShelfSpot4;

    // Store whether the names are correct
    public bool object1Correct;
    public bool object2Correct;
    public bool object3Correct;
    public bool object4Correct;

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

        if(object1Correct && object2Correct && object3Correct && object4Correct && !animationPlayed)
        {
            animationPlayed = true;
            animation.Play();
            OnPuzzleComplete(true);
        }
    }
}
