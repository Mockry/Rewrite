using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    private float timeLimit = 120f;
    private float timeRemaining = 0;
    private int timeDisplay;

    private static int activeEnemies = 0;

    private bool levelCleared = false;
    private bool levelFailed = false;

    // only the player has the PlayerHealthManager script

    Text text;

    // Start is called before the first frame update
    void Start()
    {
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
         levelFailed = true; 

        if (activeEnemies == 0 && timeRemaining <= 20f)
        {
            levelCleared = true;
        }

        text.text = "Time: " + timeDisplay + "        Enemies: " + activeEnemies;

    }

    public void AddEnemy()
    {
        activeEnemies++;
    }
    public void RemoveEnemy ()
    {
        activeEnemies--;
    }


}
