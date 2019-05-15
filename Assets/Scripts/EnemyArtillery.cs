using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArtillery : MonoBehaviour
{
    //the barrel of the shooter
    public GameObject barrel;
    //the turning part
    public GameObject turnCylinder;

    //the player body
    public GameObject playerBody;

    //bullet to fire at player
    public Transform bullet;

    public GameObject instantiateBulletLocation;

    public GameObject debug;


    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Vector3.Distance(playerBody.transform.position, instantiateBulletLocation.transform.position);
        Vector3 guessedPlayerPosition = playerBody.transform.position +
            (playerBody.transform.GetComponent<Rigidbody>().velocity * distToPlayer * 0.005f);
        turnCylinder.transform.LookAt(guessedPlayerPosition);

        debug.transform.position = guessedPlayerPosition;
        if(distToPlayer < 300)
        {
            doBullets();
        }
    }



    float lastFireTime = 0;
    public float secondsPerFire = 0.2f;
    private void doBullets()
    {
        float currTime = Time.time;
        if (currTime - lastFireTime > secondsPerFire)
        {
            lastFireTime = currTime;
            Transform clone = Instantiate(bullet, instantiateBulletLocation.transform.position, barrel.transform.rotation);
            float randomAmont = 3f;
            clone.Rotate(new Vector3(0 + UnityEngine.Random.Range(randomAmont, randomAmont), 
                0 + UnityEngine.Random.Range(-randomAmont, randomAmont), -90 + UnityEngine.Random.Range(-randomAmont, randomAmont)));
            clone.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 7000, 0));
        }
    }
}
