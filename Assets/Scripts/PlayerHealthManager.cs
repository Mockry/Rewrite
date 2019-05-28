using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealthManager : MonoBehaviour
{
    private int startingHealth = 100;
    private int currentHealth;
    private float regenTimer = 3;

    private float deathTimer = 2;
    private float deathCounter;

    // Boolean to store if the player will take damage from the time freeze
    // needs a seperate variable so it doesnt apply once per frozen enemy
    public bool freezePenalty = false;

    //damage taken when using time freeze.
    // public so the enemyController can check if you have enough health to use it
    public int freezeCost = 20;

    private AudioSource deathSound;
    private bool soundPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        //This is for putting a delay between losing all health and resetting the level
        deathCounter = deathTimer;
        // the player will only have one sound so just searching
        // for all audio sources is enough
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
     if(currentHealth <= 0)
        {
            // plays the deathSOund but stops it playing once per frame
            // probably a better way of doing this but I dont know what it is
            if(soundPlaying == false)
            {
                soundPlaying = true;
                deathSound.Play();
            }

            deathCounter -= Time.deltaTime;
            if (deathCounter <= 0)
            {       
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
        }
       
        // Gradual health Regen
            regenTimer -= Time.deltaTime;
            if (regenTimer <= 0)
            {
                currentHealth++;
                regenTimer = 2;
            }

        if (currentHealth > 100)
            currentHealth = 100;

    }

    public void HurtPlayer(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

    public int getHealth()
    {
        return currentHealth;
    }

    public void RestoreHealth()
    {
        currentHealth += 2;
    }


    private void LateUpdate()
    {
        if (freezePenalty == true)
        {
            freezePenalty = false;
            HurtPlayer(freezeCost);       
        }
    }
}
