using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatManager : MonoBehaviour
{
    private float timeLimit = 20f;
    private float timeRemaining = 0;
    private int timeDisplay;

    private static int activeEnemies ;
    public int totalWaves = 1;
    private int wavesRemaining;
    public bool pendingwave = false;

    private bool levelCleared = false;

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
        // reduces the time left to finish the level and tracks how health the player has
        timeRemaining -= Time.deltaTime;
        timeDisplay = (int)timeRemaining;

        if (timeRemaining <= 0)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        if (activeEnemies == 0 && wavesRemaining == 0)
        {
            levelCleared = true;     
            //inserted for testing
           SceneManager.LoadScene (SceneManager.GetSceneByName("Level1").buildIndex);
        }

        text.text = "Time: " + timeDisplay + "     Enemies: " + activeEnemies;

    }
    private void LateUpdate()
    {
        if (pendingwave == true)
        {
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
