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

    public UnityEvent OnTransitionComplete; // Event to notify when the transition is complete


    void Start()
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
    }

    void Update()
    {
        if (paused)
        {
            return;
        }

        if (objectRenderer != null)
        {
            elapsedTime += Time.deltaTime; 

            // Calculate the normalized time based on the duration
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Evaluate the curve at the normalized time
            float curveValue = curve.Evaluate(t);

            // Lerp between startColor and endColor using the curve value
            Color newColor = Color.Lerp(startColor, endColor, curveValue);

            // Apply the color to the object's material
            objectRenderer.material.color = newColor;
            currentColor = newColor;

            // If complete, Stop and notify
            if (t >= 1f)
            {
                elapsedTime = duration;
                OnTransitionComplete?.Invoke();     
            }
        }
    }
}

