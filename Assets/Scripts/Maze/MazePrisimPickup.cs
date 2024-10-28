using System.Collections;
using UnityEngine;

public class MazePrisimPickup : MonoBehaviour
{
    [SerializeField] PrismType prismType;
    [SerializeField] Vector3 playerTeleportLocation;
    Transform aura, player;
    [SerializeField] AnimationCurve auraScale;
    Rigidbody rb;
    Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        rb = GetComponent<Rigidbody>();
        if(prismType == PrismType.Light){
            aura = transform.GetChild(0);
            player = FindFirstObjectByType<VRController>().transform;
        }
    }
    void OnPickup(Transform hand){
        VRHudManager hud = FindFirstObjectByType<VRHudManager>();
        try{
            //hud.Debug("Picked up " + prismType.ToString() + " Prism");
            StartCoroutine(TeleportWithDelay(hand));
        }catch(System.Exception e){
            hud.Debug("Error: " + e.Message);
            hud.Debug($"Hand: {hand.name}, Parent: {hand.parent.name}, GrandParent: {hand.parent.parent.name}");
        }
    }
    IEnumerator TeleportWithDelay(Transform hand){
        yield return new WaitForSeconds(0.5f);
        hand.parent.parent.parent.GetComponent<VRController>().TeleportToPosition(playerTeleportLocation, 0);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = origin;
        if(prismType == PrismType.Light){
            float distanceToPlayer = Vector3.Distance(player.position, aura.position);
            float size = auraScale.Evaluate(distanceToPlayer);
            aura.localScale = Vector3.one * size;
        }
    }
}
[System.Serializable]
enum PrismType
{ Light, Dark}