﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherited from CatEnemy so I can use the charging mechanics
public class BossEnemy : CatEnemy

{
    private bool shielded;
    private float storedCharge;



    // Start is called before the first frame update
    void Start()
    {
        procRange = 20;
        chargeSpeed = 20;
        attackLength = 2;
        //Despite inhertitance references need to be reset

        myRB = GetComponent<Rigidbody>();
        thePlayer = FindObjectOfType<PlayerController>();
        playerHealth = FindObjectOfType<PlayerHealthManager>();
        theTracker = FindObjectOfType<StatManager>();


        //initialise these just in case
        chargeTimer = chargeDelay;
        attackAbort = attackLength;
        nmAgent.speed = moveSpeed;
        storedSpeed = nmAgent.speed;
        currentHealth = health;

    }

    //Boss enemy works the same way as the catEnemy but is invincible unless you freeze it during
    //the attacking state

    // Update is called once per frame
    override public bool Update()
    {

        // this checks for the freeze attack during the charge
        if (attacking == true)
        {
            Freeze();
        }

        // Modifies the freeze to stop the charge (base version of freeze only changes nmAgent.speed
        // but attacking doesnt use the navAgent 
        // Also allows the Boss to be attacked in this interval and lets the freeze timer keep ticking
        // while not decreasing the attackTimer
        if(attacking == true && timeFreeze == true)
        {
            shielded = false;
            myRB.velocity = (transform.forward * 0);
            Freeze();
            return true;
        }

        // keeps the shield on at all other times
        shielded = true;
        base.Update();
        return true;
    }


   


    // uses a slightly different version of hurtEnemy
    // to take the shield into account
    public override void HurtEnemy(int damage)
    {
        if (!shielded)
        currentHealth -= damage;
    }

}
