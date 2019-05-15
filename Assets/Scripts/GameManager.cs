using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{ 
    //the health
    public Scrollbar healthScrollBar;

    public Material buildingWindows;

    //health of the player
    public static float playerHealth = 1;

    //Initializes scene
    private void Start()
    {
        playerHealth = 1;
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update() {
        //set the scroll bar percent to the player health
        updateHealthScrollBar();
        //see if the player is dead or not
        seeIfPlayerDead();

        rainBowBuildings();

    }

    float startTime = 0;
    private void rainBowBuildings()
    {
        float percent = (Time.time - startTime) / 60;
        if(percent > 1)
        {
            startTime = Time.time;
        }
        buildingWindows.SetColor("_EmissionColor", Color.HSVToRGB(percent, 1, 1)); 
    }

    //whatever player health is we set to the scrollbar
    private void updateHealthScrollBar()
    {
        healthScrollBar.value = playerHealth;
    }



    //subtracts health from the player
    public static void subtractPlayerHealth(float amount)
    {
        playerHealth -= amount;
    }


    //sees if the player has less than 0 health and manages that
    private void seeIfPlayerDead()
    {
        if (playerHealth < 0 && SceneManager.GetActiveScene().name.Equals("Level1Scene"))
        {
            //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()); 
            SceneManager.LoadScene("DeathScene", LoadSceneMode.Single);
        }
    }


    public void restartLevel()
    {
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene("Level1Scene", LoadSceneMode.Single);

    }





}
