using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRKnob : MonoBehaviour
{
    [SerializeField] Vector3 localAxis;
    [SerializeField] float UnGrabDistance = 0.1f;
    public float value;
    public UnityEvent<float> OnValueChanged;

    VRHudManager hud;
    bool active = false;
    Transform hand, model;
    float angleOffset = 0;
    Vector3 axis;
    // Start is called before the first frame update
    void Start()
    {
        hud = FindObjectOfType<VRHudManager>();
        model = transform.GetChild(0);
        
    }
    void Grabbed(Transform hand)
    {
        this.hand = hand;
        axis = transform.TransformDirection(localAxis);
        axis.Normalize();
        active = true;
        // Calculate the angle of the hand position
        DrawCircle(axis, transform.position, 0.2f, 30);
        float handAngle = GetClosestPointOnRing(hand.position + hand.up, axis, transform.position, 0.1f);
        handAngle = ReAngle(handAngle);
        
        // Calculate the offset between the knob's current angle and the hand's angle
        angleOffset = value - handAngle;
    }
    void Released()
    {
        active = false; hand = null;
    }
    void Update(){
        if(active){
            float angle = GetClosestPointOnRing(hand.position + hand.up, axis, transform.position, 0.1f);
            //transform.rotation = originRotation;
            //model.localEulerAngles = axis * (-angle + angleOffset);
            hud.Debug($"rotation: {angle + angleOffset}");
            value = ReAngle(angle + angleOffset);
            OnValueChanged?.Invoke(value);
            hud.DrawLine(transform.position, hand.position + hand.up, 0.02f, Color.red);
            if(Vector3.Distance(transform.position, hand.position) > UnGrabDistance){
                Released();
            }
        }
    }
    float ReAngle(float angle){
        if(angle < 0) { angle += 360; }
        if(angle > 360) { angle -= 360; }
        return angle;
    }
    float GetClosestPointOnRing(Vector3 inputPosition, Vector3 axis, Vector3 center, float radius)
    {
        Vector3 vectorToInput = inputPosition - center;
        Vector3 projection = Vector3.ProjectOnPlane(vectorToInput, axis);
        projection.Normalize();
        hud.DrawLine(center, center + projection * radius, 0.02f, Color.green);
        DrawCircle(axis, center, radius);
        // Calculate the angle in radians
        float angleInRadians = Mathf.Atan2(projection.z, projection.x);
        //draw a line from the input position to the point on the ring
        hud.DrawLine(inputPosition, center + projection * radius, 0.02f, Color.blue);

        // Convert the angle to degrees
        return ReAngle(angleInRadians * Mathf.Rad2Deg);
    }
    void DrawCircle(Vector3 axis, Vector3 center, float radius, float duration = 0.02f)
    {
        Vector3 up = Vector3.Cross(axis, Vector3.up);
        if(up.magnitude < 0.1f) up = Vector3.Cross(axis, Vector3.right);
        up.Normalize();
        Vector3 right = Vector3.Cross(axis, up);
        for(int i = 0; i < 360; i+=10){
            float angle = i * Mathf.Deg2Rad;
            Vector3 point = center + up * Mathf.Sin(angle) * radius + right * Mathf.Cos(angle) * radius;
            hud.DrawLine(point, center + up * Mathf.Sin(angle + Mathf.Deg2Rad * 10) * radius + right * Mathf.Cos(angle + Mathf.Deg2Rad * 10) * radius, duration, Color.green);
        }
    }
}