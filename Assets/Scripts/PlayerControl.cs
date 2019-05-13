﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    Vector3 offsetCam;
    Quaternion offsetCamRot;

    public Transform lookAtMe;
    public Transform cameraLocation;
    public Transform playerBody;
    public Transform camera;
    public Transform bullet;

    //all the saved locations
    public ArrayList savedLocations = new ArrayList();

    public float movementSpeed = 1500.0f;
    public float consistentMovement = 500.0f;
    

    // Use this for initialization
    void Start () {
        //offsetCam = camera.position - playerBody.position;
    }

    
    // Update is called once per frame
    void FixedUpdate () {
        savedLocations.Add(new LocationData(transform.GetChild(0).position, transform.GetChild(0).rotation));

        if (savedLocations.Count > 2)
        {
            savedLocations.RemoveAt(0);
        }

        //camera.position = cameraLocation.position;

        //Vector3 relativePos = lookAtMe.position - cameraLocation.position;

        // the second argument, upwards, defaults to Vector3.up
        //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //camera.rotation = rotation;
        doBullets();
        playerControl();
    }


    float lastFireTime = 0;
    public float secondsPerFire = 0.2f;
    private void doBullets()
    {
        float currTime = Time.time;
        if (Input.GetAxis("Fire1") > 0.5 && currTime - lastFireTime > secondsPerFire)
        {
            lastFireTime = currTime;
            Transform clone = Instantiate(bullet, playerBody.position, playerBody.rotation);
            clone.Rotate(new Vector3(0, 0, 90));
            clone.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 7000, 0));
        }
    }
    private void playerControl()
    {
        playerBody.GetComponent<Rigidbody>()
            .AddRelativeTorque(new Vector3(Input.GetAxis("Horizontal") * 15, 0, 0));

        playerBody.GetComponent<Rigidbody>()
            .AddRelativeTorque(new Vector3(0, 0, Input.GetAxis("Vertical") * 3.5f));

        playerBody.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(-consistentMovement, 0f, 0f));
    }
}


//saves the location of an object
public class LocationData
{
    public Vector3 position;
    public Quaternion rotation;
    public LocationData(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}