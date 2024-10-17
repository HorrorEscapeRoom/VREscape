using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRController : MonoBehaviour
{
    [SerializeField] Transform NeckPivot, Cam, LHand, RHand, teleportTarget, posMat;
    [SerializeField] Transform LHip, RHip, LChest, RChest, LSholder, RSholder;
    [SerializeField] float speed = 8.0f, jumpForce = 18.0f;
    Transform teleportAimObject;
    Vector3 teleportEndPosition;
    bool teleporting = false, validTeleportTarget = false;
    LineRenderer line;
    Rigidbody rb;
    VRHudManager hud;
    Vector3 lastheadPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        hud = FindFirstObjectByType<VRHudManager>();
    }
    public void Move(Vector2 input){
        Vector3 forward = NeckPivot.forward;
        Vector3 right = NeckPivot.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 moveDirection = forward * input.y + right * input.x;
        Vector3 move = moveDirection * speed;
        move.y = rb.velocity.y;
        rb.velocity = move;
    }
    public void SnapTurn(float angle){
        Vector3 euler = transform.eulerAngles;
        euler.y += angle;
        transform.eulerAngles = euler;
        //roate the posMat the opposite direction
        euler = posMat.eulerAngles;
        euler.y -= angle;
        posMat.eulerAngles = euler;
    }
    public void InitiateTeleport(Transform pointer){
        line.enabled = true;
        teleportAimObject = pointer;
        teleporting = true;
    }
    public void Teleport(bool cancel = false){
        teleportTarget.position = Vector3.down * 1000; //send jimbo to the shadow realm
        if(cancel){ line.enabled = false; teleporting = false; }
        else if(teleporting ){
            //if we dont mark ourselves as kinematic unity says bad things about our mother
            if(validTeleportTarget){
                rb.isKinematic = true;
                transform.position = teleportEndPosition;
                rb.isKinematic = false;
            }
            line.enabled = false;
            teleporting = false;

        }
    }
    public void TeleportToPosition(Vector3 position, float newFacingDirection = -1){
        rb.isKinematic = true;
        transform.position = position;
        if(newFacingDirection != -1){
            Quaternion rot = posMat.rotation;
            Vector3 euler = transform.eulerAngles;
            euler.y = newFacingDirection;
            transform.eulerAngles = euler;
            posMat.rotation = rot; //dont rotate the posMat
        }
        rb.isKinematic = false;
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
        Vector3 offset = position - NeckPivot.position;
        hud.Debug($"Offset: {offset}");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositions();
        DrawTeleportTrace();
        NoPeek(); //Disabled For Main compatibility
    }
    void FixedUpdate(){
        
    }
    void DrawTeleportTrace(){
        if(teleporting){
            Vector3 startPos = teleportAimObject.position;
            Vector3 startDir = -teleportAimObject.forward;
            int maxIterations = 100;
            float maxDistance = 0.1f;
            bool foundMap = false;
            List<Vector3> points = new List<Vector3>();
            //raycast with gravity
            Vector3 currentPos = startPos;
            Vector3 currentDir = startDir;
            for(int i = 0; i < maxIterations; i++){
                RaycastHit hit;
                Debug.DrawRay(currentPos, currentDir * maxDistance, Color.red);
                if(Physics.Raycast(currentPos, currentDir, out hit, maxDistance, 1 << 6)){
                    points.Add(hit.point);
                    teleportEndPosition = hit.point;
                    //is the hit point a valid teleport target? (flat surface)
                    Vector3 normal = hit.normal;
                    normal.y = 0;
                    normal.Normalize();
                    float angle = Vector3.Angle(Vector3.up, normal);
                    validTeleportTarget = angle < 5;
                    teleportTarget.position = hit.point;
                    foundMap = true;
                    break;
                }
                else{
                    points.Add(currentPos + currentDir * maxDistance);
                    currentPos = points[points.Count - 1];
                    currentDir += Vector3.down * 0.03f;
                }
            }
            if(!foundMap){ validTeleportTarget = false; }
            line.positionCount = points.Count;
            line.SetPositions(points.ToArray());
        }
    }
    void UpdatePositions()
    {
        Vector3 flatForward = new Vector3(NeckPivot.forward.x, 0, NeckPivot.forward.z).normalized;
        Vector3 flatRight = new Vector3(NeckPivot.right.x, 0, NeckPivot.right.z).normalized;
        //offset based on camera forward and right
        Vector3 hipOffset = new Vector3(0.2f, -0.70f, -0.02f);
        Vector3 chestOffset = new Vector3(0.11f, -0.37f, 0.07f);
        Vector3 sholderOffset = new Vector3(-0.14f, 0f, 0.0f);

        LHip.position = CalculatePosition(NeckPivot.position, hipOffset, flatRight, flatForward);
        RHip.position = CalculatePosition(NeckPivot.position, new Vector3(-hipOffset.x, hipOffset.y, hipOffset.z), flatRight, flatForward);

        LChest.position = CalculatePosition(NeckPivot.position, chestOffset, flatRight, flatForward);
        RChest.position = CalculatePosition(NeckPivot.position, new Vector3(-chestOffset.x, chestOffset.y, chestOffset.z), flatRight, flatForward);

        LSholder.position = CalculatePosition(NeckPivot.position, sholderOffset, flatRight, flatForward);
        RSholder.position = CalculatePosition(NeckPivot.position, new Vector3(-sholderOffset.x, sholderOffset.y, sholderOffset.z), flatRight, flatForward);
    }
    void NoPeek(){
        Vector3 headOrigin = transform.position + Vector3.up * 1.5f;
        Vector3 headDirection = Cam.position - headOrigin;
        Debug.DrawRay(headOrigin, headDirection, Color.red);
        bool peeking = Physics.Raycast(headOrigin, headDirection, headDirection.magnitude, 1 << 6);

        bool inWall = Physics.CheckSphere(Cam.position, 0.13f, 1 << 6);
        bool isOOB = peeking || inWall;

        Cam.GetComponent<Camera>().enabled = !isOOB;
    }
    Vector3 CalculatePosition(Vector3 camPosition, Vector3 offset, Vector3 flatRight, Vector3 flatForward)
    { return camPosition + offset.x * flatRight + offset.z * flatForward + Vector3.up * offset.y; }
}
