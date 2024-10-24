using UnityEngine;
using UnityEngine.Events;

public class MaterialColourTransitionWithCurve : MonoBehaviour
{
    public Renderer objectRenderer; // The Renderer component of the object
    public Color startColor = Color.red; // The starting color
    public Color endColor = Color.blue; // The ending color
    public Color currentColor;
    public bool paused = true;
    public float duration = 2f; // Duration of the color change in seconds
    public AnimationCurve curve; // Animation curve to control the interpolation

    private float elapsedTime = 0f; // Time elapsed since the start of the lerp
    public float triggerThreshold = 0.5f; // The threshold on the normalized curve to trigger the event
    
    public UnityEvent OnTransitionComplete; // Event to notify when the transition is complete
    public UnityEvent OnTriggerAtThreshold; // Event for triggering at specific curve point
    
    private bool triggerFired = false; // To prevent firing the event multiple times

    
    //Allow for Detransiton back to original color.
    
    private void Start()
    {
        // Initialize the color of the object
        if (objectRenderer != null)
        {
            objectRenderer.material.color = startColor;
        }

        if (curve == null)
        {
            Debug.LogError($"MaterialColourTransitionWithCurve - AnimationCurve('Curve') is null");
        }
    }

    public void StartLerp()
    {
        paused = false;
    }

    public void StopLerp()
    {
        paused = true;
    }

    public void ResetLerp()
    {
        StopLerp();
        currentColor = startColor;
        elapsedTime = 0f;
        triggerFired = false;
    }

    private void Update()
    {
        if (paused || objectRenderer == null)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        // Calculate the normalized time based on the duration
        float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

        // Evaluate the curve at the normalized time
        float curveValue = curve.Evaluate(normalizedTime);

        // Lerp between startColor and endColor using the curve value
        Color newColor = Color.Lerp(startColor, endColor, curveValue);

        // Apply the color to the object's material
        objectRenderer.material.color = newColor;
        currentColor = newColor;

        // Check if the curve value has passed the trigger threshold
        if (!triggerFired && normalizedTime >= triggerThreshold)
        {
            OnTriggerAtThreshold?.Invoke(); // Fire the event
            triggerFired = true; // Ensure it only triggers once per lerp
        }

        // If complete, Stop and notify
        if (normalizedTime >= 1f)
        {
            elapsedTime = duration;
            OnTransitionComplete?.Invoke();
        }
    }
}

