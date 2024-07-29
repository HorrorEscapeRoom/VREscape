using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRController : MonoBehaviour
{
    [SerializeField] Transform cam, LHand, RHand;
    [SerializeField] Transform LHip, RHip, LChest, RChest, LSholder, RSholder, LBack, RBack;
    [SerializeField] float speed = 8.0f, jumpForce = 18.0f;
    Rigidbody rb;
    VRHudManager hud;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hud = FindObjectOfType<VRHudManager>();
    }
    public void Move(Vector2 input){
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 moveDirection = forward * input.y + right * input.x;
        Vector3 move = moveDirection * speed;
        move.y = rb.velocity.y;
        rb.velocity = move;
    }
    public void Jump(){
        //are we grounded?
        bool grounded = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, 0.15f);
        if(grounded){
            float useJump = jumpForce;
            float L = LHand.GetComponent<HandItemTracker>().GetVelocity(10).y;
            float R = RHand.GetComponent<HandItemTracker>().GetVelocity(10).y;
            useJump += L > 0 ? L : 0;
            useJump += R > 0 ? R : 0;
            rb.AddForce(Vector3.up * useJump, ForceMode.Impulse);
        }
    }
    public void LogOffsetFromHead(Vector3 position){
        Vector3 offset = position - cam.position;
        hud.Debug($"Offset: {offset}");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositions();
    }
    void UpdatePositions()
    {
        Vector3 flatForward = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;
        Vector3 flatRight = new Vector3(cam.right.x, 0, cam.right.z).normalized;
        //offset based on camera forward and right
        Vector3 hipOffset = new Vector3(0.2f, -0.70f, -0.02f);
        Vector3 chestOffset = new Vector3(0.11f, -0.37f, 0.07f);
        Vector3 sholderOffset = new Vector3(-0.14f, 0f, 0.0f);
        Vector3 backOffset = new Vector3(0.1f, -0.51f, -0.3f);

        LHip.position = CalculatePosition(cam.position, hipOffset, flatRight, flatForward);
        RHip.position = CalculatePosition(cam.position, new Vector3(-hipOffset.x, hipOffset.y, hipOffset.z), flatRight, flatForward);

        LChest.position = CalculatePosition(cam.position, chestOffset, flatRight, flatForward);
        RChest.position = CalculatePosition(cam.position, new Vector3(-chestOffset.x, chestOffset.y, chestOffset.z), flatRight, flatForward);

        LSholder.position = CalculatePosition(cam.position, sholderOffset, flatRight, flatForward);
        RSholder.position = CalculatePosition(cam.position, new Vector3(-sholderOffset.x, sholderOffset.y, sholderOffset.z), flatRight, flatForward);

        LBack.position = CalculatePosition(cam.position, backOffset, flatRight, flatForward);
        RBack.position = CalculatePosition(cam.position, new Vector3(-backOffset.x, backOffset.y, backOffset.z), flatRight, flatForward);
    }
    Vector3 CalculatePosition(Vector3 camPosition, Vector3 offset, Vector3 flatRight, Vector3 flatForward)
    { return camPosition + offset.x * flatRight + offset.z * flatForward + Vector3.up * offset.y; }
}
