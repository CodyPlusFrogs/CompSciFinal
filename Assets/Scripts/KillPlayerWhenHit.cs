using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerWhenHit : MonoBehaviour
{
    public PlayerControl mainPlayerControl;
    
    //dead if we hit something
    void OnCollisionEnter(Collision col)
    {
        mainPlayerControl.killPlayer();
        //meme
        GameManager.playerHealth = 0;
    }
}
