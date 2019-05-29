using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    //I want this to be public so I can assign different enemies to individual spawnpoints
    public EnemyController enemy;
    //controls interval between waves of enemies
    private float spawnDelay = 15f;
    private float spawnTimer;

    //Need this to tell the Enemies where to spawn
    public Transform spawnPoint;
    //Needs a pointer to the stat manager so it can track and display active enemies
    //The spawnpoint also needs to check if there are more waves to spawn
    private StatManager counter;

    // Start is called before the first frame update
    void Start()
    {
        //The initial spawn has a shorter Delay to start the action quickly
        spawnTimer = 5f;
        counter = FindObjectOfType<StatManager>();
        //Makes the enemies spawn facing the center of the level
        transform.LookAt(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        //WaveCount() is a getter that returns how many waves are still to spawn
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && counter.WaveCount() >= 1)
        {
            // Resets the spawnTimer and spawns an object of the Type Enemy Controller
            spawnTimer = spawnDelay;
            EnemyController newEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation) as EnemyController;

            //there is only one stat manager per level and multiple spawnpoints
            // this method allows one script to manage enemies, waves and win/losing
            counter.AddEnemy();
            counter.pendingwave = true;
        }
    }
}
