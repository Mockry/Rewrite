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

    //controls long the enemy stops before charging
    protected float chargeDelay = 1;
    protected float chargeTimer;
    // and how long the charge is
    protected float attackLength = 0.5f;
    protected float attackAbort;

    protected float chargeSpeed = 12f;
    protected float storedCharge;

    //Serialised Fields let me store several audio sources on one object
    //and set them in the editor
    [SerializeField] protected AudioSource catHit;
    [SerializeField] protected AudioSource chargeSound;
    //This has to be here as BosssEnemy works by calling the CatEnemy update
    [SerializeField] protected AudioSource sheepcharge;    

    // Start is called before the first frame update
    void Start()
    {
        procRange = 5;
        enemyType = "cat";

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
        storedCharge = chargeSpeed;
        currentHealth = health;

    }

    // base.Update(); calls the Enemy controller version of Update
    // Since this returns true at the end it stops both versions being run needlessly
    // The enemy logic is a finite state machine that changes states when the play is within a
    // set radius

    // Update is called once per frame
    override public bool Update()
    {
        // damage is applied in the base update so this stops damage from being delayed
        // Until the charge sequence ends
        // Health/Death tracker
        if (currentHealth <= 0)
        {
            thePlayer.playSound(enemyType);
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
                //Changes the enemy's state and resets the chargeTimer
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
                //controls how far the enemy charges
                attackAbort = attackLength;
                //Changes the state gain
                attacking = true;
                //plays one of the sound effects
                if(enemyType == "cat")
                    chargeSound.Play();
                if (enemyType == "sheep")
                    sheepcharge.Play();
            }
        }
        //stops the charge after a delay and restores previous speed
        if (attacking == true)
        {
            //The navAgent is very slow to respond so using velocity works better here
            //I think it has a built in easing function but upping the acceleration only helped a little
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

    // needed to override this to stop it crashing.
    // The enemy controller doesn't seem to like the serialisedfields
    override public void HurtEnemy(int damage)
    {
        catHit.Play();
        currentHealth -= damage;
    }


}
