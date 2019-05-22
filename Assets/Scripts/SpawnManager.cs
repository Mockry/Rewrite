using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    //I want this to be public so I can assign different enemies to individual spawnpoints
    public EnemyController enemy;

    private float spawnDelay = 20f;
    private float spawnTimer;

    public Transform spawnPoint;

    private StatManager counter;

    // Start is called before the first frame update
    void Start()
    {
        //The initial spawn has a short delay and the stat manager tracks active enemies
        spawnTimer = 5f;
        counter = FindObjectOfType<StatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            // Resets the spawnTimer and spawns an object of the Type Enemy Controller
            spawnTimer = spawnDelay;
            EnemyController newEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation) as EnemyController;
            counter.AddEnemy();
        }
    }
}
