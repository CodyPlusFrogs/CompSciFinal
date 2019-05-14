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

    //health of the player
    public static float playerHealth = 1;

    //Initializes scene
    private void Start()
    {
        playerHealth = 1;
    }
    // Update is called once per frame
    void Update() {
        //set the scroll bar percent to the player health
        updateHealthScrollBar();
        //see if the player is dead or not
        seeIfPlayerDead();
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
