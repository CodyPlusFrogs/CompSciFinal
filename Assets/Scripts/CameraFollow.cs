using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerControl playerControlScript = player.GetComponent<PlayerControl>();

        ArrayList allLocationData = playerControlScript.savedLocations;

        if (allLocationData.Count > 0)
        {
            Debug.Log((allLocationData[0] as LocationData).rotation);

            transform.position = (allLocationData[0] as LocationData).position;
            transform.position += transform.right * 10;
        
            transform.rotation = (allLocationData[0] as LocationData).rotation;
        }
    }
}
