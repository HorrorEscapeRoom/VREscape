using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.XR;

public class Combination : MonoBehaviour
{
    
    IRotatable rotatable;
    
    bool needReset = false;

    //int predecessor;  
    //int correctAmountRotations;  
    //int timesPredecessorPassed;  
    //int totalAmountRotations;  
    //bool complete;
    // Need to figure out how not to have circular dependencies

    public Combination(IRotatable rotatable)
    {
        this.rotatable = rotatable;
    }

    void Reset()
    {
        indexOfCorrectDigit = 0;
        needReset = false;
    }

    //void ProcessDigitDialed(ref float timer, ref float pauseDuration, float prevAngle, float absAngle)  // # combination
    //{
    //    if (!IsLockActive()) return;  // # combination

    //    if (timer >= pauseDuration)  // # combination
    //    {
    //        dialIndex = Mathf.FloorToInt(prevAngle / stepAngle);  // # rotations
    //        correctAmountRotations = CalculateCorrectRotations();  // # rotations
    //        if (IncrementIndexOfCorrectDigit(dialIndex == correctNumbers[indexOfCorrectDigit], ref timer))  // # combination
    //        {
    //            dial.Locked = true;  // # combination
    //            door.Openable = true;  // # door

    //            PlaySound(Sound.Unlocking);  // # door
    //            needReset = true;  // # combination
    //            Debug.Log($"Dialed digit processed: {dialIndex}, Correct index: {indexOfCorrectDigit}, Lock Dial: {lockDial}, Can Door Open: {canDoorOpen}");  // # combination
    //        }
    //        else PlaySound(Sound.IncorrectNumber);  // # combination
    //    }
    //}

    public void ProcessDigitDialed(int digitDialed)
    {
        if (digitDialed == correctNumbers[indexOfCorrectDigit])
        {
            indexOfCorrectDigit++;
            if (indexOfCorrectDigit == correctNumbers.Count - 1)
            {
                needReset = true;
            }
        }
    }


    void Reset(ref float prevAngle, ref float absAngle)
    {
        indexOfCorrectDigit = 0;  // # combination
        totalAmountRotations = 0;  // # combination
        correctAmountRotations = 0;  // # combination
        timesPredecessorPassed = 0;  // # combination
        needReset = false;  // # combination
        prevAngle = absAngle;
        Debug.Log("Combination reset.");  // # combination
    }

    int CalculateCorrectRotations()  // # rotations
    {
        int currentAngle = correctNumbers[indexOfCorrectDigit] * stepAngle;  // # combination
        int previousAngle = correctNumbers[Mathf.Max(indexOfCorrectDigit - 1, 0)] * stepAngle;  // # combination
        float angleDifference = currentAngle - previousAngle;  // # rotations
        int rotations = Mathf.FloorToInt(angleDifference / 360);  // # rotations
        Debug.Log($"Current Angle: {currentAngle}, Previous Angle: {previousAngle}, Calculated Rotations: {rotations}");  // # rotations
        return rotations;  // # rotations
    }

    bool IncrementIndexOfCorrectDigit(bool numberDialedCorrect, ref float timer)  // # combination
    {
        UpdateTimerIfCondition(ref timer, numberDialedCorrect);  // # combination
        if (numberDialedCorrect)  // # combination
        {
            PlaySound(Sound.CorrectNumber);  // # combination
            predecessor = correctNumbers[(indexOfCorrectDigit - 1 + correctNumbers.Count) % correctNumbers.Count];  // # combination
            if (indexOfCorrectDigit != correctNumbers.Count - 1)  // # combination
            {
                indexOfCorrectDigit++;  // # combination
                Debug.Log($"Index of correct digit incremented: {indexOfCorrectDigit}");  // # combination
                return true;  // # combination
            }
        }
        return false;  // # combination
    }

    

    bool AllConditionsMatch(float angleDifference)  // # combination
    {
        if (!door.Openable) handPositionZ = hand.transform.position.z;

        bool conditionsMatch = !door.Opened && !lockDial && !door.Openable &&  // # door
                               dialIndex == correctNumbers[indexOfCorrectDigit] &&  // # combination
                               totalAmountRotations == rotations.CurrentAmount &&  // # rotations
                               IsExpectedDirection(angleDifference) &&  // # rotations
                               timesPredecessorPassed == indexOfCorrectDigit;  // # combination

        Debug.Log($"Checking all conditions match: {conditionsMatch}");  // # combination
        return conditionsMatch;  // # combination
    }

    // Increment the count of times the predecessor has been passed.
    void IncrementPredecessorCount(ref int timesPredecessorPassed, float angleDifference)  // # rotations
    {
        if (dialIndex > predecessor && IsExpectedDirection(angleDifference))  // # rotations
            timesPredecessorPassed++;  // # combination
        Debug.Log($"Predecessor count: {timesPredecessorPassed}, Predecessor: {predecessor}, Current Number: {dialIndex}");  // # combination
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code here.
    }

    // Update is called once per frame
    void Update()
    {
        // Update code here.
    }
}