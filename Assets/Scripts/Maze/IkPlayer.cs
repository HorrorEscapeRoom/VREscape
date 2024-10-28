using System.Collections;
using UnityEngine;

public class IkPlayer : MonoBehaviour
{
    [SerializeField] Transform PlayerRoot, Head, Neck, Lefthand, Righthand;
    Transform ModelRoot, ModelHead, ModelNeck, ModelLefthand, ModelRighthand,
    ModelSpine1, ModelSpine2, ModelLShoulder, ModelLArm1, ModelLArm2, 
    ModelRShoulder, ModelRArm1, ModelRArm2;
    // Start is called before the first frame update
    void Start()
    {
        ModelRoot = transform.GetChild(0);
        ModelSpine1 = ModelRoot.GetChild(2);
        ModelSpine2 = ModelSpine1.GetChild(0);
        ModelNeck = ModelSpine2.GetChild(0);
        ModelHead = ModelNeck.GetChild(0);
        ModelLShoulder = ModelSpine2.GetChild(1);
        ModelLArm1 = ModelLShoulder.GetChild(0);
        ModelLArm2 = ModelLArm1.GetChild(0);
        ModelLefthand = ModelLArm2.GetChild(0);
        ModelRShoulder = ModelSpine2.GetChild(2);
        ModelRArm1 = ModelRShoulder.GetChild(0);
        ModelRArm2 = ModelRArm1.GetChild(0);
        ModelRighthand = ModelRArm2.GetChild(0);
        StartCoroutine(TickMovement());
    }

    IEnumerator TickMovement(){
        while(true){
            yield return new WaitForSeconds(0.1f);
            //spine
            ModelSpine1.position = ModelNeck.position + Vector3.down * 0.2f;
            ModelSpine2.position = ModelNeck.position + Vector3.down * 0.1f;
            //neck and head
            Vector3 relativeNeck = Neck.position - PlayerRoot.position;
            ModelNeck.position = ModelRoot.position + relativeNeck;
            ModelNeck.rotation = Neck.rotation;
            Vector3 relativeHead = Head.position - Neck.position;
            ModelHead.position = ModelNeck.position + relativeHead;
            ModelHead.rotation = Head.rotation;
            //left arm
            Quaternion convertedLHandRot = ConvertHandRotation(Lefthand.rotation);
            Vector3 relativeLHand = Lefthand.position - PlayerRoot.position;
            Vector3 EndHandPos = ModelRoot.position + relativeLHand;
            ModelLShoulder.position = Vector3.Lerp(ModelNeck.position, EndHandPos, 0.1f);
            ModelLArm1.position = Vector3.Lerp(ModelNeck.position, EndHandPos, 0.3f);
            ModelLArm1.rotation = Quaternion.Lerp(ModelLShoulder.rotation, convertedLHandRot, 0.3f);
            ModelLArm2.position = Vector3.Lerp(ModelNeck.position, EndHandPos, 0.5f);
            ModelLArm2.rotation = Quaternion.Lerp(ModelLShoulder.rotation, convertedLHandRot, 0.5f);
            ModelLefthand.position = EndHandPos;
            ModelLefthand.rotation = convertedLHandRot;
            
            //right arm
            Vector3 relativeRHand = Righthand.position - PlayerRoot.position;
            EndHandPos = ModelRoot.position + relativeRHand;
            ModelRShoulder.position = Vector3.Lerp(ModelNeck.position, EndHandPos, 0.1f);
            ModelRArm1.position = Vector3.Lerp(ModelNeck.position, EndHandPos, 0.3f);
            ModelRArm2.position = Vector3.Lerp(ModelNeck.position, EndHandPos, 0.5f);
            ModelRighthand.position = EndHandPos;
        }
    }
    Quaternion ConvertHandRotation(Quaternion handRotation){
        //player hands use Z+ forward Y+ up
        //model hands use Y+ forward X+ up
        Vector3 forward = handRotation * Vector3.forward;
        Vector3 up = handRotation * Vector3.up;
        return Quaternion.LookRotation(forward, up);
    }
}
