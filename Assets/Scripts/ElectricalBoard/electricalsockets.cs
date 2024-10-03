using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class ElectricalSockets : MonoBehaviour
{
    public List<GameObject> snapPositionObjects;
    public List<GameObject> plugEnds;
    public float snapThreshold = 0.1f;
    public Material greenMat;
    public Material redMat;

    private Dictionary<GameObject, Rigidbody> plugEndRigidbodies = new Dictionary<GameObject, Rigidbody>();
    public List<GameObject> snappedlist;

    GameObject playerbody;

    public bool light1on = false;
    public bool light2on = false;
    public bool light3on = false;

    public GameObject[] light1snaps = new GameObject[2];
    public GameObject light1;
    public GameObject[] light2snaps = new GameObject[2];
    public GameObject light2;
    public GameObject[] light3snaps = new GameObject[2];
    public GameObject light3;

    void Start()
    {
        // Pre-calculate the squared snap threshold to avoid square root calculation in Update

        // Cache Rigidbody components

        //playerbody =  Find the player or set public for now

        playerbody = GameObject.Find("VRRIG");

        foreach (GameObject plugEnd in plugEnds)
        {
            if (plugEnd != null)
            {
                plugEndRigidbodies[plugEnd] = plugEnd.GetComponent<Rigidbody>();
            }
        }
    }

    void Update()
    {
        float playerDistance = (playerbody.transform.position - transform.position).magnitude;

        if (playerDistance < 2f) // 2f squared is 4
        {
            foreach (GameObject plugEnd in plugEnds)
            {
                if (plugEnd != null)
                {
                    Rigidbody targetRigidbody = plugEndRigidbodies[plugEnd];
                    bool isSnapped = false;
                    Vector3 plugEndPosition = plugEnd.transform.position;

                    foreach (GameObject snapObject in snapPositionObjects)
                    {
                        if (snapObject != null)
                        {
                            float distance = (plugEndPosition - snapObject.transform.position).magnitude;

                            if (distance <= snapThreshold)
                            {
                                if (plugEnd.name == "ropePlugEnd")
                                {
                                    plugEnd.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                                }
                                else if (plugEnd.name == "ropePlugStart")
                                {
                                    plugEnd.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
                                }

                                plugEnd.transform.position = snapObject.transform.position + new Vector3 (0f,0f,-0.05f);
                                isSnapped = true;

                                if (!snappedlist.Contains(snapObject))
                                {
                                    snappedlist.Add(snapObject);
                                    checkLights();
                                }
                                break;
                            }
                        }
                    }

                    if (isSnapped)
                    {
                        if (targetRigidbody != null && (!targetRigidbody.isKinematic || targetRigidbody.useGravity))
                        {
                            targetRigidbody.isKinematic = true;
                            targetRigidbody.useGravity = false;
                        }
                    }
                    else
                    {
                        if (targetRigidbody != null && (targetRigidbody.isKinematic || !targetRigidbody.useGravity))
                        {
                            targetRigidbody.isKinematic = false;
                            targetRigidbody.useGravity = true;
                        }

                        foreach (GameObject snapObject in snapPositionObjects)
                        {
                            if (snapObject != null)
                            {
                                float distance = (plugEndPosition - snapObject.transform.position).magnitude;

                                if (distance > snapThreshold && snappedlist.Contains(snapObject))
                                {
                                    snappedlist.Remove(snapObject);
                                    checkLights();
                                }
                            }
                        }

                    }
                }
            }
        }
    }

    void checkLights() 
    {
        if (snappedlist.Contains(light1snaps[0]) && snappedlist.Contains(light1snaps[1])) { light1.GetComponent<MeshRenderer>().material = greenMat; light1on = true ;} else { light1.GetComponent<MeshRenderer>().material = redMat; light1on = false ; };
        if (snappedlist.Contains(light2snaps[0]) && snappedlist.Contains(light2snaps[1])) { light2.GetComponent<MeshRenderer>().material = greenMat; light2on = true ;} else { light2.GetComponent<MeshRenderer>().material = redMat; light2on = false; };
        if (snappedlist.Contains(light3snaps[0]) && snappedlist.Contains(light3snaps[1])) { light3.GetComponent<MeshRenderer>().material = greenMat; light3on = true ;} else { light3.GetComponent<MeshRenderer>().material = redMat; light3on = false; };

        if (light1on && light2on && light3on) 
        { 
        // puzzle complete
        }
    }

private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (GameObject snapObject in snapPositionObjects)
        {
            if (snapObject != null)
            {
                Gizmos.DrawSphere(snapObject.transform.position, 0.1f);
            }
        }
        Gizmos.color = Color.blue;
        foreach (GameObject targetObject in plugEnds)
        {
            if (targetObject != null)
            {
                Gizmos.DrawWireSphere(targetObject.transform.position, 0.1f);
            }
        }
    }
}