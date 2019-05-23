using System.Collections;
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
    }

    // Update is called once per frame
    override public bool Update()
    {

        // this checks for the freeze attack during the charge
        if (attacking == true)
        {
            Freeze();
        }

        //Modifies the freeze to stop the charge (base version only changes nmAgent.speed)
        //Also allows the Boss to be attacked in this interval lets the freezeTimer keep ticking
        if(attacking == true && timeFreeze == true)
        {
            shielded = false;
            myRB.velocity = (transform.forward * 0);
            Freeze();
            return true;
        }

        //
        shielded = true;
        base.Update();
        return true;
    }


   


    //uses a slightly different version of hurt enem
    public override void HurtEnemy(int damage)
    {
        if (!shielded)
        currentHealth -= damage;
    }

}
