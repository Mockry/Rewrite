using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    protected Rigidbody myRB;
    public float moveSpeed = 3;
    protected float storedSpeed;


    //This is for controlling which sounds get played
    protected string enemyType;

    //the enemy only needs to target the player and only the player has a PlayerController Script
    //protected so the child classes can access it
    protected PlayerController thePlayer;
    protected PlayerHealthManager playerHealth;

    //Controls the duration of the time freeze
    protected float freezeTimer;
    protected float freezeLength = 3;
    //Sets if the time freeze is active or not
    protected bool timeFreeze = false;

    //Health Manager. health is public so I can change it in the editor
    public int health;
    protected int currentHealth;

    protected StatManager theTracker;
  
    public NavMeshAgent nmAgent;
    
    protected AudioSource enemyHit;

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
        //sets default health
        currentHealth = health;

        //This class only has one sound effect on it so GetComponent works just fine
        enemyHit = GetComponent<AudioSource>();
        //This tells the player what sound to play when the enemy dies
        //(the enemy can't play it as the audio source vanishes when it dies)
        enemyType = "basic";
    }

   public virtual void FixedUpdate()
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

        //Changed my original movement method to one that uses pathfinding
        nmAgent.SetDestination(thePlayer.transform.position);

        //Checks if the Time freeze is being used
        //This needs to be seperate from the update for BossEnemy's shield to work
        Freeze();

        // Health/Death tracker
        if (currentHealth <= 0)
        {
            //Tells the player what deathSound to play
            //Restores some of the players health
            //Kills the enemy
            //Updates the active enemies 
            thePlayer.playSound(enemyType);
            playerHealth.RestoreHealth();
            Destroy(gameObject);
            theTracker.RemoveEnemy();
        }
        return true;
    }

   public virtual void Freeze()
    {
        //Checks for right mouse button or left bumper being pressed
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Joystick1Button4))
            // Also checks if the time freeze is already active and you have enough health to use it
            && timeFreeze == false 
            && playerHealth.getHealth() > playerHealth.freezeCost)
        {
            // stops movement while the timefreeze is active and starts the timer
            timeFreeze = true;
            freezeTimer = freezeLength;
            nmAgent.speed = 0.0f;
            moveSpeed = 0;

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
            moveSpeed = storedSpeed;
        }
    }

    //this needs to be virtual so I can change the way the BossEnemy takes damage later
    public virtual void HurtEnemy(int damage)
    {
        enemyHit.Play();
        currentHealth -= damage;
    }

}
