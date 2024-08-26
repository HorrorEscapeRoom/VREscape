using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EzFunctionCall))]
public class EzFunctionCallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EzFunctionCall ezFunctionCall = (EzFunctionCall)target;
        if (GUILayout.Button("Call Function"))
        {
            ezFunctionCall.CallFunction();
        }
    }
}