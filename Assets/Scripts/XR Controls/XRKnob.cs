using UnityEngine;
using UnityEngine.Events;

public class XRKnob : MonoBehaviour
{
    [SerializeField] float UnGrabDistance = 0.1f;
    [SerializeField] float angle;
	
	public UnityEvent<float> OnValueChanged;

	[SerializeField] bool hasLimits = false;
	[SerializeField] float minLimitAngle = 0.0f;
	[SerializeField] float maxLimitAngle = 360.0f;
	

	VRHudManager hud;
    bool active = false;
    Transform hand, model;

	float handAngleLastFrame = 0;

	float deltaLastFrame = 0;
	float angleLastFrame = 0;


	// Start is called before the first frame update
	void Start()
    {
        hud = FindObjectOfType<VRHudManager>();
        model = transform.GetChild(0);
		if(hasLimits){
			angle = Mathf.Clamp(angle, minLimitAngle, maxLimitAngle);
		}
		UpdateMeshRotation();
	}
    void Grabbed(Transform hand)
    {
        this.hand = hand;
		handAngleLastFrame =  GetHandThing();
        active = true;
    }
    void Released()
    {
		active = false; hand = null;
    }
    void Update(){

        if(active){

			float currentAngle = GetHandThing();
			float deltaAngle = currentAngle - handAngleLastFrame;
			if(hasLimits){
				if(deltaAngle + angle > maxLimitAngle){
					//Debug.Log($"base:{angle}, delta:{deltaAngle}, sum:{angle + deltaAngle} exceedes max: {maxLimitAngle}");
					angle = maxLimitAngle;
				}else if(deltaAngle + angle < minLimitAngle){
					//Debug.Log($"base: {angle}, delta:{deltaAngle}, sum:{angle + deltaAngle} is less than min: {minLimitAngle}");
					angle = minLimitAngle;
				}else{
					angle += deltaAngle;
				}
			}else{
				angle += deltaAngle;
			}
			angle = ReAngle(angle);
			handAngleLastFrame = currentAngle;

			if(angle - angleLastFrame > 15){
				//we probably snapped Unexpectedly/ full rotation
				Debug.Log($"snapped: last: {angleLastFrame}, current: {angle}, delta: {angle - angleLastFrame}");
			}

			deltaLastFrame = deltaAngle;
			angleLastFrame = angle;

			UpdateMeshRotation();

			OnValueChanged?.Invoke(angle);
            hud.DrawLine(transform.position, hand.position + hand.up, 0.02f, Color.red);
            if(Vector3.Distance(transform.position, hand.position) > UnGrabDistance){
                Released();
            }
        }
    }

	void UpdateMeshRotation() {
		model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.x, -angle, model.localRotation.eulerAngles.z);
	}

	float GetHandThing() {
		Vector3 handAngleInLocalSpace = transform.InverseTransformPoint(hand.position + hand.up);
		handAngleInLocalSpace.y = 0;
		handAngleInLocalSpace.Normalize();
		return Mathf.Atan2(handAngleInLocalSpace.z, handAngleInLocalSpace.x) * Mathf.Rad2Deg;
	}

    float ReAngle(float angle){
		while(angle < 0) { angle += 360; }
		while(angle > 360) { angle -= 360; }
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