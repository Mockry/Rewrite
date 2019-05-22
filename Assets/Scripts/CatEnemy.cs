using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// sets up CatEnemy as a child of Enemy controller

    //Apparently inhereting this way just lets me re use the constructor
    // I still need to copy over the functions
public class CatEnemy : EnemyController
{
    //booleans controlling the attacking and ready to attack states.
    private bool attacking = false;
    private bool charging = false;
    //stores the player location at the when it comes in range
    private Vector3 chargeTarget;

    //controls the charge time and charge duration
    private float chargeDelay = 2;
    private float chargeTimer;

    private float attackLenght = 1;
    private float attackAbort;

    private float chargeSpeed = 10;
    

    // Start is called before the first frame update
    void Start()
    {

        //Despite inhertitance values need to be re initialised
        
        myRB = GetComponent<Rigidbody>();
        thePlayer = FindObjectOfType<PlayerController>();
        playerHealth = FindObjectOfType<PlayerHealthManager>();
        theTracker = FindObjectOfType<StatManager>();


        //initialise these just in case
        chargeTimer = chargeDelay;
        attackAbort = attackLenght;
        nmAgent.speed = moveSpeed;
        storedSpeed = nmAgent.speed;
    }

   //base.Update(); calls the Enemy controller version of Update
   //Since this returns true at the end it stops both versions being run needlessly

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
            if (dist <= 4)
            {
                //stops the enemy when in range of the player and stores the players loacation
                nmAgent.speed = 0;
                chargeTarget = thePlayer.transform.position;
                charging = true;
                chargeTimer = chargeDelay;
                return true;
            }
            base.Update();
        }        
        
        //sets the target to the players location and increases speed 
        // after a delay
        if (charging == true && attacking == false)
        {
            chargeTimer -= Time.deltaTime;
            if (chargeTimer <= 0)
            {
                attackAbort = attackLenght;
                nmAgent.SetDestination(chargeTarget);
                nmAgent.speed = chargeSpeed;
                attacking = true;
            }
        }

        //stops the charge after a delay and restores previous speed
        if (attacking == true)
        {
            attackAbort -= Time.deltaTime;
            if (attackAbort <= 0)
            {
                nmAgent.speed = storedSpeed;
                attacking = false;
                charging = false;

            }
        }
        

        return true;
    }

    public void HurtEnemy(int damage)
    {
        currentHealth -= damage;
    }

}
