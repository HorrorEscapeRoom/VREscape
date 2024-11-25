using UnityEngine;
using UnityEngine.Events;

public class XRKnob : MonoBehaviour
{
    [SerializeField] float UnGrabDistance = 0.1f;
    public UnityEvent<float> OnValueChanged;

    private Transform hand;
    private Transform model;

    private float BaseAngle = 0.0f;
    private float angleOffset = 0.0f;
    private float initGrabHandAngle = 0.0f;
    private bool active = false;

    public Transform Hand => hand; 

    public float AbsAngle => ReAngle(BaseAngle + angleOffset); 

    void Start()
    {
        model = transform.GetChild(0);
        UpdateMeshRotation();
    }

    public void Grabbed(Transform grabbingHand)
    {
        hand = grabbingHand;
        initGrabHandAngle = GetHandAngle();
        active = true;
    }

    public void Released()
    {
        BaseAngle = AbsAngle;
        active = false;
        hand = null;
    }

    void Update()
    {
        if (!active) return;

        angleOffset = initGrabHandAngle - GetHandAngle();
        UpdateMeshRotation();
        OnValueChanged?.Invoke(AbsAngle);

        if (Vector3.Distance(transform.position, hand.position) > UnGrabDistance)
        {
            Released();
        }
    }

    private void UpdateMeshRotation()
    {
        model.localRotation = Quaternion.Euler(model.localRotation.eulerAngles.x, AbsAngle, model.localRotation.eulerAngles.z);
    }

    private float GetHandAngle()
    {
        Vector3 foobar = transform.InverseTransformPoint(hand.position + hand.up);
        foobar.y = 0;
        foobar.Normalize();
        return Mathf.Atan2(foobar.z, foobar.x) * Mathf.Rad2Deg;
    }

    private float ReAngle(float angle) => (angle + 360) % 360;
}