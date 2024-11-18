using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisController : MonoBehaviour
{
    //bc it looks nicer
    [SerializeField] AnimationCurve visCurve;
    Material mat;
    Color baseColor;
    void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        baseColor = mat.color;
    }
    public void VisUpdate(float visability){
        if(mat == null) { return; } //fix a race condition
        baseColor.a = visCurve.Evaluate(visability);
        //Debug.Log($"Visability: {visability}");
        mat.color = baseColor;
    }
}
