using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherited from CatEnemy so I can use the charging mechanics
public class BossEnemy : CatEnemy

{
    //Boolean to control when the boss is vulnerable to damage
    private bool shielded;

    // Start is called before the first frame update
    void Start()
    {
        procRange = 20;
        chargeSpeed = 25;
        chargeDelay = 3;
        attackLength = 1.5f;
        enemyType = "sheep";

        freezeLength = 5;

        // references need to be reset

        myRB = GetComponent<Rigidbody>();
        thePlayer = FindObjectOfType<PlayerController>();
        playerHealth = FindObjectOfType<PlayerHealthManager>();
        theTracker = FindObjectOfType<StatManager>();
        enemyHit = GetComponent<AudioSource>();


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

        // Modifies the freeze to stop the charge (base version of freeze only changes movespeed
        // and nmAgent. speed but attacking uses chargeSpeed.
        // Also allows the Boss to be attacked in this interval and lets the freeze timer keep ticking
        // while not decreasing the attackTimer and ending the charge
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
        {
            enemyHit.Play();
            currentHealth -= damage;
        }
    }
}
