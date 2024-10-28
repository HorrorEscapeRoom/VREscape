using UnityEngine;

public class ControlPassthough : MonoBehaviour
{
    public void Grabbed(Transform hand)
    {
        transform.parent.BroadcastMessage("OnGrab", hand);
    }
    public void Released()
    {
        transform.parent.BroadcastMessage("OnRelease");
    }
}
