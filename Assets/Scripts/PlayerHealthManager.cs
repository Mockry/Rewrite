using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealthManager : MonoBehaviour
{
    public int startingHealth;
    private int currentHealth;
    private float regenTimer = 3;

    public float flashLength;
    private float flashCounter;

    // Boolean to store if the player will take damage from the time freeze
    // needs a seperate variable so it doesnt apply once per frozen enemy
    public bool freezePenalty = false;

    //damage taken when using time freeze.
    // public so the enemyController can check if you have enough health to use it
    public int freezeCost = 20;

    private Renderer rend;
    private Color storedColor;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        rend = GetComponent<Renderer>();
        storedColor = rend.material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
     if(currentHealth <= 0)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        if (flashCounter > 0)
        {
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                rend.material.SetColor("_Color", storedColor);
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
        flashCounter = flashLength;
        rend.material.SetColor("_Color", Color.white);
    }

    public int getHealth()
    {
        return currentHealth;
    }

    public void RestoreHealth()
    {
        currentHealth += 5;
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
