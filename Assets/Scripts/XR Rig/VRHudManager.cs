using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRHudManager : MonoBehaviour
{
    Transform cam;
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
    public void SetTouchingText(bool Rhand, string touching){
        if(Rhand == false){
            leftTouchingText.text = touching;
        }
        else{
            rightTouchingText.text = touching;
        }
    }
}
