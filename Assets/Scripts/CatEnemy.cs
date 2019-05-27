using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// sets up CatEnemy as a child of Enemy controller
public class CatEnemy : EnemyController
{

    //Set date to protected as my bossEnemy will use the same charge mechanic

    //booleans controlling the attacking and ready to attack states.
    protected bool attacking = false;
    protected bool charging = false;

    //sets how close the player has to be to trigger the charge
    protected float procRange;
    //stores the player location at the when it comes in range
    protected Vector3 chargeTarget;

    //controls the charge time and charge duration
    protected float chargeDelay = 2;
    protected float chargeTimer;

    protected float attackLength = 0.5f;
    protected float attackAbort;

    protected float chargeSpeed = 10f;
    

    // Start is called before the first frame update
    void Start()
    {
        procRange = 5;

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

    //base.Update(); calls the Enemy controller version of Update
    //Since this returns true at the end it stops both versions being run needlessly
    // The enemy logic is a finite state machine that changes states when the play is within a
    // set radius

    // Update is called once per frame
    override public bool Update()
    {
        // damage is applied in the base update so this stops damage from being delayed
        //Until the charge sequence ends
        // Health/Death tracker
        if (currentHealth <= 0)
        {
            playerHealth.RestoreHealth();
            Destroy(gameObject);
            theTracker.RemoveEnemy();
            return true;
        }

        // uses the base update function when not attacking

        if (charging == false)
        {  
            //Gets the distance between the enemy and the player
            float dist = Vector3.Distance(thePlayer.transform.position, transform.position);
            if (dist <= procRange)
            {
                //stops the enemy when in range of the player and stores the players loacation
                nmAgent.speed = 0;
                chargeTarget = thePlayer.transform.position;
                charging = true;
                chargeTimer = chargeDelay;
                return true;
            }
            // runs the Enemy Controller update while out of range
            // which returns true and ends the update here
            base.Update();
        }        
        
        //sets the target to the players location and increases speed 
        // after a delay
        if (charging == true && attacking == false)
        {
             transform.LookAt(chargeTarget);
       
            chargeTimer -= Time.deltaTime;
            if (chargeTimer <= 0)
            {
                attackAbort = attackLength;
                attacking = true;
            }
        }

        //stops the charge after a delay and restores previous speed
        if (attacking == true)
        {
            //Did the charge this way becuase the nmAgent got confused
            // if the enemy charged into an object
            myRB.velocity = (transform.forward * chargeSpeed);

            // stops the charge after a set duration
            attackAbort -= Time.deltaTime;
            if (attackAbort <= 0)
            {
                //this resets the velocity so the nmAgent can take over again
                myRB.velocity = (transform.forward * 0);
                nmAgent.speed = storedSpeed;
                //resets the state so it starts running the baseUpdate
                attacking = false;
                charging = false;
            }
        }

        return true;
    }
    // Stops the charge if the Enemy hits an obstacle
    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Blocker")
            attackAbort = 0;
    }

}
