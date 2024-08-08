using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRHudManager : MonoBehaviour
{
    Transform cam;
    [SerializeField] GameObject LineDebugPref;
    [SerializeField] TMP_Text debugText, leftTouchingText, rightTouchingText;
    List<string> messages = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //lerp the position of the HUD to the camera
        transform.position = Vector3.Lerp(transform.position, cam.position, 0.1f);
        transform.rotation = Quaternion.Lerp(transform.rotation, cam.rotation, 0.1f);
    }

    public void Debug(string message){
        messages.Add(message);
        //only show the last 10 messages
        if(messages.Count > 8){
            messages.RemoveAt(0);
        }
        string textString = "";
        foreach(string msg in messages){
            textString += msg + "\n";
        }
        debugText.text = textString;
    }
    public void DrawLine(Vector3 start, Vector3 end, float duration, Color color){
        GameObject g = Instantiate(LineDebugPref);
        Gradient g2 = new Gradient();
        GradientColorKey s1 = new GradientColorKey(); s1.color = color; s1.time = 0;
        GradientColorKey s2 = new GradientColorKey(); s2.color = color; s2.time = 1;
        g2.colorKeys = new GradientColorKey[]{s1,s2};
        g.GetComponent<LineRenderer>().SetPositions(new Vector3[]{start, end});
        g.GetComponent<LineRenderer>().colorGradient = g2;
        Destroy(g, duration);
    }
    public void SetTouchingText(bool Rhand, string touching){
        if(Rhand == false){
            leftTouchingText.text = touching;
        }
        else{
            rightTouchingText.text = touching;
        }
    }
}
