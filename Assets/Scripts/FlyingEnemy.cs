﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyController
{
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        thePlayer = FindObjectOfType<PlayerController>();
        playerHealth = FindObjectOfType<PlayerHealthManager>();
        theTracker = FindObjectOfType<StatManager>();

        currentHealth = health;

    }


    //Acts the same as the basic enemy but doesn't use the navMesh
    // So it ignores obstables (didn't actually work till I changed it's colldier to be a trigger)


    public override void FixedUpdate()
    {
        myRB.velocity = (transform.forward * moveSpeed);
    }
    // Update is called once per frame
   public override bool Update()
    {
        transform.LookAt(thePlayer.transform.position);
        Freeze();

        if (currentHealth <= 0)
        {
            //Restores some of the players health
            //Kills the enemy
            //Updates the active enemies
            playerHealth.RestoreHealth();
            Destroy(gameObject);
            theTracker.RemoveEnemy();
        }

        return true;
    }
}