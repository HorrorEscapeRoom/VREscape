using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRKnob : MonoBehaviour
{
    [SerializeField] float UnGrabDistance = 0.1f;
    [SerializeField] int amountDialNumbers;
    [SerializeField] List<int> correctNumbers = new();
    [SerializeField] float pauseDuration = new();
    [SerializeField] AudioSource[] audioSorces;

    enum Sound
    {
        DoorMoving,
        CorrectNumber,
        IncorrectNumber,
        Locking,
        Unlocking
    }

    int maxDoorOpenAngle = 180;

    GameObject door;
    public UnityEvent<float> OnValueChanged;
    VRHudManager hud;
    bool active = false;
    Transform hand, model;
    float handRotationY;
    float handPositionZ;
    int dialIndex;
    float baseAngleKnob = 0.0f;
    float angleOffset = 0.0f;
    int stepAngle;
    float initGrabHandAngel = 0.0f;
    const float EPSILON = 1.0f;
    float prevAngle;
    bool canDoorOpen = false;
    bool lockDial = false;
    int indexOfCorrectDigit = 0;
    bool needReset = false;
    int predecessor;
    bool doorOpened = false;
    bool lockSoundPlayed;
    float doorsInitialYRotation;
    int totalAmountRotations;
    int correctAmountRotations;
    int timesPredecessorPassed;
    float timerHandRotation = 0.0f;
    float timerHandDisplacement = 0.0f;
    float pauseTimer = 0.0f;

    float AbsAngle { get { return ReAngle(baseAngleKnob + angleOffset); } }

    void Start()
    {
        hud = FindObjectOfType<VRHudManager>();
        model = transform.GetChild(0);
        stepAngle = 360 / amountDialNumbers;
        door = transform.parent.gameObject;
        doorsInitialYRotation = door.transform.rotation.y;
    }

    float GetHandThing()
    {
        Vector3 foobar = transform.InverseTransformPoint(hand.position + hand.up);
        foobar.y = 0;
        foobar.Normalize();
        float result = Mathf.Atan2(foobar.z, foobar.x) * Mathf.Rad2Deg;
        Debug.Log($"GetHandThing result: {result}");
        return result;
    }

    void Grabbed(Transform hand)
    {
        this.hand = hand;
        initGrabHandAngel = GetHandThing();
        active = true;
        handRotationY = hand.transform.rotation.y;
        Debug.Log("Hand grabbed; initialization angle set.");
    }

    void Released()
    {
        baseAngleKnob = AbsAngle;
        active = false;
        hand = null;
        Debug.Log("Hand released; base angle updated.");
    }

    void PlaySound(Sound soundType)
    {
        if (!audioSorces[(int)soundType].isPlaying)
        {
            audioSorces[(int)soundType].Play();
            Debug.Log($"Playing sound: {soundType}");
        }
    }

    int CalculateCorrectRotations()
    {
        int currentAngle = correctNumbers[indexOfCorrectDigit] * stepAngle;
        int previousAngle = correctNumbers[Mathf.Max(indexOfCorrectDigit - 1, 0)] * stepAngle;
        float angleDifference = currentAngle - previousAngle;
        int rotations = Mathf.FloorToInt(angleDifference / 360);
        Debug.Log($"Current Angle: {currentAngle}, Previous Angle: {previousAngle}, Calculated Rotations: {rotations}");
        return rotations;
    }

    bool IsLockActive() => !doorOpened && !lockDial;

    bool UpdateDoorState()
    {
        doorOpened = door.transform.eulerAngles.z > 0;
        lockDial = !doorOpened;
        Debug.Log($"Door state updated: DoorOpened={doorOpened}, LockDial={lockDial}");
        return doorOpened;
    }

    bool IsHandNearby() => Vector3.Distance(transform.position, hand.position) >= UnGrabDistance;

    void ResetCombination(ref float prevAngle, ref float absAngle)
    {
        indexOfCorrectDigit = 0;
        totalAmountRotations = 0;
        correctAmountRotations = 0;
        timesPredecessorPassed = 0;
        needReset = false;
        prevAngle = absAngle;
        Debug.Log("Combination reset.");
    }

    void UpdateTimerIfCondition(ref float timer, bool condition)
    {
        if (condition) timer += Time.deltaTime;
        else timer = 0f;
    }

    bool IncrementIndexOfCorrectDigit(bool numberDialedCorrect, ref float timer)
    {
        UpdateTimerIfCondition(ref timer, numberDialedCorrect);
        if (numberDialedCorrect)
        {
            PlaySound(Sound.CorrectNumber);
            predecessor = correctNumbers[(indexOfCorrectDigit - 1 + correctNumbers.Count) % correctNumbers.Count];
            if (indexOfCorrectDigit != correctNumbers.Count - 1)
            {
                indexOfCorrectDigit++;
                Debug.Log($"Index of correct digit incremented: {indexOfCorrectDigit}");
                return true;
            }
        }
        return false;
    }

    void ProcessDigitDialed(ref float timer, ref float pauseDuration, float prevAngle, float absAngle)
    {
        if (!IsLockActive()) return;

        if (timer >= pauseDuration)
        {
            dialIndex = Mathf.FloorToInt(prevAngle / stepAngle);
            correctAmountRotations = CalculateCorrectRotations();
            if (IncrementIndexOfCorrectDigit(dialIndex == correctNumbers[indexOfCorrectDigit], ref timer))
            {
                lockDial = true;
                canDoorOpen = true;
                PlaySound(Sound.Unlocking);
                needReset = true;
                Debug.Log($"Dialed digit processed: {dialIndex}, Correct index: {indexOfCorrectDigit}, Lock Dial: {lockDial}, Can Door Open: {canDoorOpen}");
            }
            else PlaySound(Sound.IncorrectNumber);
        }
    }

    bool IsExpectedDirection(float angleDiff)
    {
        bool expectedDirection = (angleDiff > 0) == (indexOfCorrectDigit % 2 == 0);
        Debug.Log($"Expected direction: {expectedDirection}, Index: {indexOfCorrectDigit}");
        return expectedDirection;
    }

    bool AllConditionsMatch(float angleDifference)
    {
        if (!canDoorOpen) handPositionZ = hand.transform.position.z;

        bool conditionsMatch = !doorOpened && !lockDial && !canDoorOpen &&
                               dialIndex == correctNumbers[indexOfCorrectDigit] &&
                               totalAmountRotations == correctAmountRotations &&
                               IsExpectedDirection(angleDifference) && timesPredecessorPassed == indexOfCorrectDigit;

        Debug.Log($"Checking all conditions match: {conditionsMatch}");
        return conditionsMatch;
    }

    void IncrementPredecessorCount(ref int timesPredecessorPassed, float angleDifference)
    {
        if (dialIndex > predecessor && IsExpectedDirection(angleDifference)) timesPredecessorPassed++;
        Debug.Log($"Predecessor count: {timesPredecessorPassed}, Predecessor: {predecessor}, Current Number: {dialIndex}");
    }

    void HandleDoorRotation()
    {
        float currentYRotation = door.transform.rotation.y;
        float diffInitCurrentYRotation = currentYRotation - doorsInitialYRotation;
        if (currentYRotation == doorsInitialYRotation && doorOpened)
        {
            PlaySound(Sound.Locking);
            canDoorOpen = false;
            Debug.Log("Door opened");
        }
        else
        {
            if (diffInitCurrentYRotation > 0 && diffInitCurrentYRotation < 180 || diffInitCurrentYRotation < 0)
            {
                PlaySound(Sound.DoorMoving);
                Debug.Log("Handling door rotation.");
            }
        }
    }

    bool ApplyRotation(ref float absAngle, ref float timer, float deltaAngle, Transform model = null, GameObject targetObject = null)
    {
        if (deltaAngle > EPSILON)
        {
            prevAngle = absAngle;
            if (targetObject == null)
            {
                model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.x, absAngle, transform.localRotation.eulerAngles.z);
            }
            else targetObject.transform.Rotate(0, absAngle / timer * Time.deltaTime, 0);

            angleOffset = initGrabHandAngel - GetHandThing();
            OnValueChanged?.Invoke(absAngle);
            hud.DrawLine(transform.position, hand.position + hand.up, 0.02f, Color.red);

            Debug.Log($"Applying rotation: AbsAngle={absAngle}, DeltaAngle={deltaAngle}");
            return true;
        }
        else timer = 0f;
        return false;
    }

    void CheckAndApplyRotation(float absAngle, float angleDifference)
    {
        if (ApplyRotation(ref absAngle, ref pauseTimer, angleDifference, model))
        {
            if (AllConditionsMatch(angleDifference))
            {
                ProcessDigitDialed(ref pauseTimer, ref pauseDuration, prevAngle, absAngle);
                IncrementPredecessorCount(ref timesPredecessorPassed, angleDifference);
            }
            totalAmountRotations = Mathf.FloorToInt((angleDifference) / 360.0f);
            Debug.Log($"Checking rotation: AbsAngle={absAngle}, Angle Difference={angleDifference}");
        }
    }

    void HandleRotationUpdate()
    {
        float absAngle = AbsAngle;
        float angleDifference = absAngle - prevAngle;

        if (!active)
        {
            if (!IsHandNearby())
            {
                Released();
                return;
            }
            else Grabbed(hand);
            if (UpdateDoorState())
            {
                handPositionZ = hand.transform.position.z;
                needReset = true;
                return;
            }
            else CheckAndApplyRotation(absAngle, angleDifference);
            if (needReset) ResetCombination(ref prevAngle, ref absAngle);
            if (canDoorOpen)
            {
                HandleDoorRotation();
            }
        }
        Debug.Log("Updating rotation handling.");
    }

    void Update() => HandleRotationUpdate();

    float ReAngle(float angle)
    {
        if (angle < 0) angle += 360;
        if (angle > 360) angle -= 360;
        return angle;
    }

    void DrawCircle(Vector3 axis, Vector3 center, float radius, float duration = 0.02f)
    {
        Vector3 up = Vector3.Cross(axis, Vector3.up);
        if (up.magnitude < 0.1f) up = Vector3.Cross(axis, Vector3.right);
        up.Normalize();
        Vector3 right = Vector3.Cross(axis, up);
        for (int i = 0; i < 360; i += 10)
        {
            float angle = i * Mathf.Deg2Rad;
            Vector3 point = center + up * Mathf.Sin(angle) * radius + right * Mathf.Cos(angle) * radius;
            hud.DrawLine(point, center + up * Mathf.Sin(angle + Mathf.Deg2Rad * 10) * radius + right * Mathf.Cos(angle + Mathf.Deg2Rad * 10) * radius, duration, Color.green);
        }
    }

    void debug_draw_axis(Vector3 point, Vector3 axis, Color col) =>
        hud.DrawLine(point, point + (axis * 5.0f), 1000.0f, col);
}