using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    LineRenderer lineRenderer;
    ParticleSystem splashParticle;

    Coroutine pourRoutine;
    Coroutine splashRoutine;
    Vector3 targetPos = Vector3.zero;
    
    Coroutine fillRoutine;
    public GameObject fillLocation;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
        //fillLocation = transform.parent.GetComponent<PourDetector>().fillLocation;
    }

    public void Begin()
    {
        pourRoutine = StartCoroutine(nameof(BeginPour));
        //StartCoroutine(BeginPour());
        splashRoutine = StartCoroutine(UpdateParticle());
        /*if(fillRoutine != null)
        {

        }
            fillLocation.SetActive(true);
            fillRoutine = StartCoroutine(FluidFilling());*/
    }

    IEnumerator BeginPour()
    {
        while (gameObject.activeSelf)
        {
            targetPos = FindEndPoint();
            MoveToPosition(0, transform.position);
            AnimateToPosition(1, targetPos);

            yield return null;
        }

    }

    public void End()
    {
        StopCoroutine(pourRoutine);
        StopCoroutine(splashRoutine);
        //StopCoroutine(fillRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    IEnumerator EndPour()
    {
        while(!HasReachedPosition(0, targetPos))
        {
            AnimateToPosition(0, targetPos);
            AnimateToPosition(1, targetPos);
            yield return null;
        }
        Destroy(gameObject);
    }

    Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Physics.Raycast(ray, out hit, 2.0f);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);
        return endPoint;
    }

    void MoveToPosition(int index, Vector3 targetPosition)
    {
        lineRenderer.SetPosition(index, targetPosition);
    }

    public void AnimateToPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPos = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(index, newPos);
    }

    bool HasReachedPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPos = lineRenderer.GetPosition(index);
        return currentPos == targetPosition;
    }

    IEnumerator UpdateParticle()
    {
        while(gameObject.activeSelf)
        {
            splashParticle.gameObject.transform.position = targetPos;

            bool isHitting = HasReachedPosition(1, targetPos);
            splashParticle.gameObject.SetActive(isHitting);
            yield return null;
        }
    }

    IEnumerator FluidFilling()
    {
        while (gameObject.activeSelf)
        {
            fillLocation.transform.position = new Vector3(targetPos.x, targetPos.y,targetPos.z);
            bool isHitting = HasReachedPosition(1, targetPos);
            yield return null;
        }
    }
}
