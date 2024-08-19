using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using UnityEngine.XR;
using static UnityEditor.FilePathAttribute;

public class XRKnob : MonoBehaviour
{
    [SerializeField] float UnGrabDistance = 0.1f;
    public float value;
    public UnityEvent<float> OnValueChanged;

    VRHudManager hud;
    bool active = false;
    Transform hand, model;

	float BaseAngle = 0.0f;
	float angleOffset = 0.0f;

	float initGrabHandAngel = 0.0f;

	float absAngle { get { return ReAngle(BaseAngle + angleOffset); } }


	// Start is called before the first frame update
	void Start()
    {
        hud = FindObjectOfType<VRHudManager>();
        model = transform.GetChild(0);
		UpdateMeshRotation();
	}
    void Grabbed(Transform hand)
    {
        this.hand = hand;

		initGrabHandAngel = GetHandThing();
		
        active = true;
    }
    void Released()
    {
		BaseAngle = absAngle;
		active = false; hand = null;
    }
    void Update(){

        if(active){

			angleOffset = initGrabHandAngel - GetHandThing();

			UpdateMeshRotation();

			OnValueChanged?.Invoke(absAngle);
            hud.DrawLine(transform.position, hand.position + hand.up, 0.02f, Color.red);
            if(Vector3.Distance(transform.position, hand.position) > UnGrabDistance){
                Released();
            }
        }
    }

	void UpdateMeshRotation() {
		model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.x, absAngle, model.localRotation.eulerAngles.z);
	}

	float GetHandThing() {

		Vector3 foobar = transform.InverseTransformPoint(hand.position + hand.up);
		foobar.y = 0;
		foobar.Normalize();

		return Mathf.Atan2(foobar.z, foobar.x) * Mathf.Rad2Deg;
	}

    float ReAngle(float angle){
        if(angle < 0) { angle += 360; }
        if(angle > 360) { angle -= 360; }
        return angle;
    }

	private void DrawCircle(Vector3 axis, Vector3 center, float radius, float duration = 0.02f)
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

	private void debug_draw_axis(Vector3 point, Vector3 axis, Color col){
		hud.DrawLine(point, point + (axis*5.0f),1000.0f , col);
		
	}

}