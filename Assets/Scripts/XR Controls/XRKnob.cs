using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRKnob : MonoBehaviour
{
    [SerializeField] float UnGrabDistance = 0.1f;
    [SerializeField] int amountDialNumbers;
    [SerializeField] List<int> correctNumbers = new();
    [SerializeField] float pauseDuration = new();
    [SerializeField] GameObject door;
    [SerializeField] AudioSource doorMoving;
    [SerializeField] AudioSource correctNumber;
    [SerializeField] AudioSource incorrectNumber;
    [SerializeField] AudioSource locking;
    [SerializeField] AudioSource unlocking;
    [SerializeField] AudioSource combinationComplete;
    [SerializeField] GameObject safe;
    [SerializeField] float speedDial;
    [SerializeField] float speedDoor;
    int maxDoorOpenAngle = 180;

    public UnityEvent<float> OnValueChanged;
    VRHudManager hud;
    bool active = false;
    Transform hand, model;
    int dialIndex;
    float prevDoorRotation;
    float baseAngle = 0.0f;
    float angleOffset = 0.0f;
    int stepAngle;
    float initGrabHandAngel = 0.0f;
    const float EPSILON = 1.0f;
    float prevAngle;
    bool canDoorOpen = false;
    int indexOfCorrectDigit = 0;
    string correctDirRotationDial = "CLK";
    string currentDirRotationDial;

    int totalAmountRotations;
    int correctAmountRotations;

    Dictionary<int, int> timesNumberPassed = new();

    float timer = 0.0f;

    float absAngle { get { return ReAngle(baseAngle + angleOffset); } }

    void Start()
    {
        InitializeComponents();
        stepAngle = 360 / amountDialNumbers;
        prevDoorRotation = door.transform.rotation.z;
        correctAmountRotations = CalculateCorrectRotations();
    }

    void InitializeComponents()
    {
        hud = FindObjectOfType<VRHudManager>();
        model = transform.GetChild(0);
        doorMoving = GetComponent<AudioSource>();
        correctNumber = GetComponent<AudioSource>();
        incorrectNumber = GetComponent<AudioSource>();
        locking = GetComponent<AudioSource>();
        unlocking = GetComponent<AudioSource>();
        combinationComplete = GetComponent<AudioSource>();
        door = door.gameObject.GetComponent<GameObject>();
    }

    void Grabbed(Transform hand) 
    {
        this.hand = hand;
        initGrabHandAngel = GetHandThing();
        active = true;
    }

    void Released() 
    {
        baseAngle = absAngle;
        active = false;
        hand = null;
    }

    bool IsDoorOpen() => door.transform.eulerAngles.z > 0;

    void PlaySound(AudioSource sound)
    {
        if (!sound.isPlaying) sound.Play();
    }

    int prevDialNumber = -1;  

    void HandleRotationUpdate()
    {
        UpdateRotation();
        if (Vector3.Distance(transform.position, hand.position) > UnGrabDistance)
        {
            Released();
            return;
        }

        if (Mathf.Abs(absAngle - prevAngle) > EPSILON)
        {
            prevAngle = absAngle;
            if (timer >= pauseDuration)
            {
                ProcessDigitDialed();
                timer = 0f;
            }

            int currentDialNumber = Mathf.FloorToInt(absAngle / stepAngle) % amountDialNumbers;

            if (currentDialNumber != prevDialNumber)
            {
                if (timesNumberPassed.ContainsKey(currentDialNumber))
                {
                    timesNumberPassed[currentDialNumber]++;
                }
                else
                {
                    timesNumberPassed[currentDialNumber] = 1;
                }

                prevDialNumber = currentDialNumber;
            }

            totalAmountRotations = Mathf.FloorToInt((absAngle - prevAngle) / 360.0f);
            PlaySound(IsNumberDialedCorrect() ? correctNumber : incorrectNumber);
            ValidateConditions();
        }

        timer += Time.deltaTime;
    }

    void UpdateRotation()
    {
        angleOffset = (initGrabHandAngel - GetHandThing()) * speedDial;
        UpdateMeshRotation();
        OnValueChanged?.Invoke(absAngle);
        hud.DrawLine(transform.position, hand.position + hand.up, 0.02f, Color.red);
    }

    int CalculateCorrectRotations()
    {
        int currentAngle = correctNumbers[indexOfCorrectDigit] * stepAngle;
        int previousAngle = correctNumbers[Mathf.Max(indexOfCorrectDigit - 1, 0)] * stepAngle;
        float angleDifference = currentAngle - previousAngle;
        return Mathf.FloorToInt(angleDifference / 360);
    }

    void ValidateConditions()
    {
        if (IsDoorOpen()) return;

        currentDirRotationDial = absAngle - prevAngle < 0 ? "CCW" : "CLK";
        if (!IsNumberDialedCorrect()) ResetCombinationState();
    }

    bool IsNumberDialedCorrect() =>
        dialIndex == correctNumbers[indexOfCorrectDigit] &&
        totalAmountRotations == correctAmountRotations;

    void ResetCombinationState()
    {
        if (IsDoorOpen()) return;

        canDoorOpen = false;
        locking.Play();
        ResetForNextDigit();
        timesNumberPassed.Clear();
    }

    void ResetForNextDigit()
    {
        indexOfCorrectDigit = (indexOfCorrectDigit + 1) % correctNumbers.Count;
        correctDirRotationDial = (indexOfCorrectDigit % 2 == 0) ? "CLK" : "CCW";
        correctAmountRotations = CalculateCorrectRotations();
    }

    void ProcessDigitDialed()
    {
        if (IsDoorOpen()) return;

        dialIndex = Mathf.FloorToInt(prevAngle / stepAngle);
        if (IsNumberDialedCorrect())
        {
            if (indexOfCorrectDigit == correctNumbers.Count - 1)
            {
                PlaySound(combinationComplete);
                PlaySound(unlocking);
                canDoorOpen = true;
            }
            else ResetForNextDigit();
        }
        else ResetCombinationState();
    }

    void Update()
    {
        if (!active)
        {
            if (IsHandNearby()) Grabbed(hand);
        }
        else
        {
            HandleRotationUpdate();
            if (canDoorOpen) UpdateDoorRotation();
        }
    }

    void UpdateDoorRotation()
    {
        float currentDoorRotation = door.transform.eulerAngles.z;
        currentDoorRotation = Mathf.Lerp(
            prevDoorRotation,
            maxDoorOpenAngle,
            speedDoor * Time.deltaTime
        );
        door.transform.eulerAngles = new Vector3(
            door.transform.eulerAngles.x,
            door.transform.eulerAngles.y,
            currentDoorRotation
        );
    }

    void UpdateMeshRotation() =>
        model.localRotation = Quaternion.Euler(
            model.localRotation.eulerAngles.x,
            absAngle,
            model.localRotation.eulerAngles.z
    );

    float GetHandThing()
    {
        Vector3 foobar = transform.InverseTransformPoint(hand.position + hand.up);
        foobar.y = 0;
        foobar.Normalize();
        return Mathf.Atan2(foobar.z, foobar.x) * Mathf.Rad2Deg;
    }

    float ReAngle(float angle)
    {
        if (angle < 0) angle += 360;
        if (angle > 360) angle -= 360;
        return angle;
    }

    bool IsHandNearby()
    {
        float handDistance = Vector3.Distance(transform.position, hand.position);
        return handDistance <= UnGrabDistance; 
    }

    private void DrawCircle(Vector3 axis, Vector3 center, float radius, float duration = 0.02f)
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

    private void debug_draw_axis(Vector3 point, Vector3 axis, Color col)
    {
        hud.DrawLine(point, point + (axis * 5.0f), 1000.0f, col);
    }
}