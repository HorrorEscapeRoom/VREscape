using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMirrorWalk : MonoBehaviour
{
    [SerializeField] Transform player, ground;
    [SerializeField] Transform[] groundMirrors;
    [SerializeField] Transform[] shadowPlayers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.z > 290 && player.position.z < 720)  
        {
            Vector3 playerPositionInGroundSpace = ground.InverseTransformPoint(player.position);
            Quaternion playerRotationInGroundSpace = Quaternion.Inverse(ground.rotation) * player.rotation;
            for(int i = 0; i < groundMirrors.Length; i++)
            {
                //move the shadow player to the same position as the player in the ground space
                shadowPlayers[i].position = groundMirrors[i].TransformPoint(playerPositionInGroundSpace);
                shadowPlayers[i].rotation = groundMirrors[i].rotation * playerRotationInGroundSpace;

            }
        }else{
            Debug.Log("Player is not in the mirror zone");
        }
    }
}
