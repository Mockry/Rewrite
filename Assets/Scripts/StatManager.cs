using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatManager : MonoBehaviour
{
    //Sets the time limit for the level and displays it on the UI
    private float timeLimit = 20f;
    private float timeRemaining = 0;
    private int timeDisplay;
    //puts a delay between clearing the level and loading the next one
    private float nextLevelTimer = 5;

    private static int activeEnemies ;
    public int totalWaves = 1;
    private int wavesRemaining;
    public bool pendingwave = false;

    public string nextLevel;

    // only the player has the PlayerHealthManager script

    Text text;

    // Start is called before the first frame update
    void Start()
    {
        activeEnemies = 0;
        wavesRemaining = totalWaves;
        text = GetComponent<Text>();
        timeRemaining = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        // reduces the time left to finish the level and displays it on screen
        timeRemaining -= Time.deltaTime;
        timeDisplay = (int)timeRemaining;

        //reloads the current scene if you run out of time
        if (timeRemaining <= 0)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        if (activeEnemies == 0 && wavesRemaining == 0)
        {
            // gives you a few seconds before loading the next level
            nextLevelTimer -= Time.deltaTime;
            if(nextLevelTimer <= 0)
           SceneManager.LoadScene (nextLevel);
        }
        

        //Sets the text displayed on the UI object
        text.text = "Time: " + timeDisplay + "     Enemies: " + activeEnemies;

    }
    private void LateUpdate()
    {
        if (pendingwave == true)
        {
            //reduces the number of remaining waves
            //pending wave is set in the enemy spawner script
            // so the late update makes sure newWave() only runs once per frame
            pendingwave = false;
            newWave();
        }
    }

    public void AddEnemy()
    {
        activeEnemies++;
    }
    public void RemoveEnemy ()
    {
        activeEnemies--;
    }
    public void newWave()
    {
        wavesRemaining--;
    }
    public int WaveCount()
    {
        return wavesRemaining;
    }

}
