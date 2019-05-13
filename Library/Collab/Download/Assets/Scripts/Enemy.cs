using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //the player that we are hunting
    public Transform playerTransform;
    //the bullet we will fire at the player
    public Transform bullet;

    //link to the laser transform
    public GameObject laser;

    //how far in front of the player the enemy will try to be
    public float maxGetInFrontOfPlayerDist = 50;
    //the current distance we aim to be in front of the player
    private float currGetInFrontOfPlayerDist;
    //everything is bigger and crazier when far away from the player
    float currCrazynessScale;



    //use for init stages
    private bool stageFinished = true;
    //the current state
    private enemyStates enemyState = enemyStates.followPlayer;
    enum enemyStates
    {
        followPlayer,
        fire
    }


    //the last time 
    private float lastRandomizePositionTime = 0;
    //the relative position from the target that we are going to (creates movement)
    private Vector2 lastRandomizePosition = new Vector2(0, 0);
    //the max amount we are allowed to move in either dimension from the target
    public float maxRandomizePosition = 10;
    

	// Use this for initialization
	void Start () {
        //we will start far away
        currGetInFrontOfPlayerDist = maxGetInFrontOfPlayerDist;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //current time in seconds
        float currTime = Time.time;


        if (enemyState == enemyStates.followPlayer)
        {
            if (stageFinished)
            {
                initializeStateVariables();
            }

            
            //point towards the player
            pointTowardsPlayer();
            laser.SetActive(false);



            bool done = harassPlayerMovement();

            if (done)
            {
                nextStage();
            }
        }

        if(enemyState == enemyStates.fire)
        {

            laser.SetActive(true);

            if (stageFinished)
            {
                initializeStateVariables();
            }
            harassPlayerMovement();

            
        }
	}

    /**
     * Gets the direction to the player and scale
     */
    private Vector3 getDirectionToPlayer(Vector3 firstPoint)
    {
        return playerTransform.position - firstPoint;
    }

    /**
     * Gets where the direction to the player if we imagine ahead of time
     */
    private Vector3 getDirectionToPlayerAheadOfTime(Vector3 firstPoint, float timeInFuture)
    {
        Vector3 playerVelocity = playerTransform.GetComponent<Rigidbody>().velocity;
        Vector3 playerInFuture = playerTransform.position + playerVelocity * timeInFuture;

        return playerInFuture - firstPoint;
    }


    /**
     * Harasses the player with random movment
     * @return if we are done or not
     */
    private bool harassPlayerMovement()
    {
        float currTime = Time.time;

        //everything is less crazy when we are close
        currCrazynessScale = currGetInFrontOfPlayerDist / maxGetInFrontOfPlayerDist;

        //we want to be infront of the player so they can see us
        Vector3 targetPosition = playerTransform.position - playerTransform.right * currGetInFrontOfPlayerDist
            + (playerTransform.up * lastRandomizePosition.x * currCrazynessScale)//add randomization in the local up
            + (playerTransform.forward * lastRandomizePosition.y * currCrazynessScale);//add randomization in the local forward

        //point towards the player
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        //get the distance to the player
        double distanceToTarget = Vector3.Distance(directionToTarget, transform.position);
        GetComponent<Rigidbody>().AddForce(directionToTarget * currCrazynessScale * 1000.0f);

        if (currTime - lastRandomizePositionTime > 0.5f)
        {
            lastRandomizePositionTime = currTime;
            lastRandomizePosition = new Vector2(UnityEngine.Random.Range(-maxRandomizePosition, maxRandomizePosition),
                UnityEngine.Random.Range(-maxRandomizePosition, maxRandomizePosition));


            //now we can get closer towards the player
            if (currGetInFrontOfPlayerDist > maxGetInFrontOfPlayerDist * 0.19f)
            {
                currGetInFrontOfPlayerDist -= UnityEngine.Random.Range(2.0f, 5.0f);
            }
            else
            {
                //now that we have reached our min distance we have a 1 in 4 chance to be done (attack the player)
                if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.75f)
                {
                    return true;
                }
            }
        }
        return false;
    }






    /**
     * Looks towards the player object
     */
    private void pointTowardsPlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }









    //inits vars once per state change
    private void initializeStateVariables()
    {
        stageFinished = false;
    }
    private void nextStage()
    {
        enemyState++;
        stageFinished = true;
    }
}
