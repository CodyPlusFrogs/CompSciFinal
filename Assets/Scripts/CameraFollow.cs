using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    //the current following state
    private static followingStates followState = followingStates.followingPlayer;
    public enum followingStates
    {
        followingPlayer,
        watchingPlayerDie
    }

    
    public GameObject player;

    // Update is called once per frame
    void FixedUpdate()
    {

        if(followState == followingStates.followingPlayer)
        {
            PlayerControl playerControlScript = player.GetComponent<PlayerControl>();

            ArrayList allLocationData = playerControlScript.savedLocations;

            if (allLocationData.Count > 0)
            {
                transform.position = (allLocationData[0] as LocationData).position;
                transform.position += transform.right * 10;

                transform.rotation = (allLocationData[0] as LocationData).rotation;
            }
        }
        if(followState == followingStates.watchingPlayerDie)
        {

            PlayerControl playerControlScript = player.GetComponent<PlayerControl>();

            ArrayList allLocationData = playerControlScript.savedLocations;

            if (allLocationData.Count > 0)
            {
                //transform.position = (allLocationData[0] as LocationData).position;
                //transform.position += transform.right * 10;
                transform.position += transform.right * -2;
                transform.GetChild(0).transform.LookAt(player.transform.GetChild(0).transform.position,transform.up);
                //transform.rotation = (allLocationData[0] as LocationData).rotation;
            }



        }
        
    }

    //watches the player die while maintaining the same camera location
    public static void watchPlayerDie()
    {
        followState = followingStates.watchingPlayerDie;
    }
}
