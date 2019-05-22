using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    protected Rigidbody myRB;
    public float moveSpeed = 3;

    //the enemy only needs to target the player and only the player has a PlayerController Script
    //protected so the child classes can access it
    protected PlayerController thePlayer;
    protected PlayerHealthManager playerHealth;

    protected float storedSpeed;
    protected float freezeTimer;
    protected float freezeLength = 3;
    protected bool timeFreeze;

    //Health Manager
    protected int health = 5;
    public int currentHealth;

    protected StatManager theTracker;


    public NavMeshAgent nmAgent;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        //searches the level for an object with PlayerController and health manager
        thePlayer = FindObjectOfType<PlayerController>();
        playerHealth = FindObjectOfType<PlayerHealthManager>();
        // stores the base move speed so it can be reset after the time freeze
        nmAgent.speed = moveSpeed;
        storedSpeed = moveSpeed;
        //sets tracker so the active enemies can be updated in the UI
        theTracker = FindObjectOfType<StatManager>();
        currentHealth = health;
    }

    void FixedUpdate()
    {
        // original movement code which I'm leaving in case I need 
        // to copy it for other enemies

        // myRB.velocity = (transform.forward * moveSpeed);
    }

    // Update is called once per frame
   virtual public bool Update()
    {  
        //other part of my old movement
        
        //finds the player position and makes the enemy face it
        //unlike the mouse pointer the enemy and player will be at the same height
        //so it doesnt need to be adjusted
        //   transform.LookAt(thePlayer.transform.position);

        //Changed my original movement to use a navmesh
        nmAgent.SetDestination(thePlayer.transform.position);


        //Time Freeze Mechanic
        // Time freeze only works if it isnt currently active and you have enough health to use it
        //without dying
        if (Input.GetMouseButtonDown(1) && timeFreeze == false && playerHealth.getHealth() > playerHealth.freezeCost)
        { 
            // stops movement while the timefreeze is active and starts the timer
            timeFreeze = true;
            freezeTimer = freezeLength;
            nmAgent.speed = 0.0f;

            //sets a pending health penalty in the healthmanager so that
            //the penalty doesn't apply once per enemy
            playerHealth.freezePenalty = true;
            
        }
        if (freezeTimer > 0)
            freezeTimer -= Time.deltaTime;

        if (freezeTimer <= 0)
        {
            //allows the freeze to be used again and puts the enemies speed back to its original value
            timeFreeze = false;
            nmAgent.speed = storedSpeed;
        }

        // Health/Death tracker
        if (currentHealth <= 0)
        {
            playerHealth.RestoreHealth();
            Destroy(gameObject);
            theTracker.RemoveEnemy();
        }
        return true;
    }

   

    public void HurtEnemy(int damage)
    {
        currentHealth -= damage;
    }

}
